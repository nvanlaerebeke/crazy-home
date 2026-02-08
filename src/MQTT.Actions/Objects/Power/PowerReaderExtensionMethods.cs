using System.Globalization;

namespace MQTT.Actions.Objects.Power;

internal static class PowerReaderExtensionMethods {
    // DSMR Reader split-topic expects plain numeric payloads as strings (e.g. "1.285")
    // We normalize using invariant culture.
    public static string? AsKwh(this RecordValueDto? v) {
        if (v is null) return null;

        // Your DTO uses strings like "Kilowatt" for energy totals (actually kWh)
        // If you have different naming, adjust here.
        // Common possibilities: "kWh", "KilowattHour", "Kilowatt"
        var unit = v.Unit?.Trim();

        if (!TryParseDecimal(v.Value, out var d)) return null;

        return unit switch {
            "kWh" => d.ToString("0.###", CultureInfo.InvariantCulture),
            "KilowattHour" => d.ToString("0.###", CultureInfo.InvariantCulture),
            "Kilowatt" => d.ToString("0.###", CultureInfo.InvariantCulture), // matches your earlier RecordUnit.Kilowatt
            _ => null
        };
    }

    public static string? AsKwFromWattOrKw(this RecordValueDto? v) {
        if (v is null) return null;

        var unit = v.Unit?.Trim();
        if (!TryParseDecimal(v.Value, out var d)) return null;

        return unit switch {
            "kW" => d.ToString("0.###", CultureInfo.InvariantCulture),
            "Kilowatt" => d.ToString("0.###", CultureInfo.InvariantCulture),

            // Your earlier parsing stored instantaneous power in Watts
            "W" => (d / 1000m).ToString("0.###", CultureInfo.InvariantCulture),
            "Watt" => (d / 1000m).ToString("0.###", CultureInfo.InvariantCulture),

            _ => null
        };
    }

    public static string? AsCubicMeters(this RecordValueDto? v) {
        if (v is null) return null;

        var unit = v.Unit?.Trim();
        if (!TryParseDecimal(v.Value, out var d)) return null;

        return unit switch {
            "m3" => d.ToString("0.###", CultureInfo.InvariantCulture),
            "m³" => d.ToString("0.###", CultureInfo.InvariantCulture),
            "CubicMeter" => d.ToString("0.###", CultureInfo.InvariantCulture),
            _ => null
        };
    }

    public static string? AsTimestamp(this RecordValueDto? v) {
        if (v is null) return null;

        // DSMR Reader usually publishes the timestamp string as-is.
        // Your format looks like YYMMDDhhmmssX (X = W/S)
        // So we just pass through.
        return string.IsNullOrWhiteSpace(v.Value) ? null : v.Value.Trim();
    }

    private static bool TryParseDecimal(string? s, out decimal value) {
        return decimal.TryParse(
            s,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value);
    }
}
