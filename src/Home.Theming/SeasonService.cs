namespace Home.Theming;

internal sealed class SeasonService {
    public Season GetCurrentSeason() {
        var today = DateOnly.FromDateTime(DateTime.Today);

        foreach (var season in Enum.GetValues<Season>()) {
            var (start, end) = GetDates(season);
            if (IsInRange(today, start, end))
                return season;
        }

        // If nothing matched (gaps in your calendar), return the next upcoming by start date.
        return GetNextSeason(today);
    }

    public (DateOnly Start, DateOnly End) GetDates(Season season) {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return season switch {
            Season.Christmas => (new DateOnly(today.Year, 12, 8), new DateOnly(today.Year, 01, 25)),
            Season.Valentine => (new DateOnly(today.Year, 01, 25), new DateOnly(today.Year, 02, 20)),
            Season.Winter => (new DateOnly(today.Year, 02, 21), new DateOnly(today.Year, 03, 15)),
            Season.Easter => (new DateOnly(today.Year, 03, 15), new DateOnly(today.Year, 04, 15)),
            Season.Spring => (new DateOnly(today.Year, 04, 15), new DateOnly(today.Year, 06, 20)),
            Season.Summer => (new DateOnly(today.Year, 06, 21), new DateOnly(today.Year, 09, 14)),
            Season.Oktoberfest => (new DateOnly(today.Year, 09, 15), FirstSundayOfOctober(today.Year)),
            Season.Fall => (FirstSundayOfOctober(today.Year).AddDays(1), FirstSundayOfOctober(today.Year).AddDays(2)),
            Season.Halloween => (FirstSundayOfOctober(today.Year).AddDays(3), new DateOnly(today.Year, 11, 30)),
            Season.Sinterklaas => (new DateOnly(today.Year, 12, 1), new DateOnly(today.Year, 12, 7)),
            _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
        };
    }

    private static DateOnly FirstSundayOfOctober(int year) {
        var d = new DateOnly(year, 10, 1);
        int offset = ((int)DayOfWeek.Sunday - (int)d.DayOfWeek + 7) % 7;
        return d.AddDays(offset);
    }

    private static bool IsInRange(DateOnly d, DateOnly start, DateOnly end) {
        // Normal window
        if (end >= start) return d >= start && d <= end;

        // Wraps over year-end (e.g., Dec 8 -> Jan 25)
        return d >= start || d <= end;
    }

    private Season GetNextSeason(DateOnly today) {
        Season? best = null;
        int bestDelta = int.MaxValue;

        foreach (var s in Enum.GetValues<Season>()) {
            var (start, _) = GetDates(s);

            // If the window wraps and today is in Jan/Feb, the start might be in Dec (past).
            // Normalize "start" to the next occurrence on/after today by bumping a year if needed.
            var normalizedStart = start;
            if (normalizedStart < today && !IsInRange(today, start, GetDates(s).End)) {
                normalizedStart = new DateOnly(start.Year + 1, start.Month, start.Day);
            }

            var delta = normalizedStart.DayNumber - today.DayNumber;
            if (delta >= 0 && delta < bestDelta) {
                bestDelta = delta;
                best = s;
            }
        }

        // Fallback (shouldn't happen if at least one season exists)
        return best ?? Enum.GetValues<Season>()[0];
    }
}
