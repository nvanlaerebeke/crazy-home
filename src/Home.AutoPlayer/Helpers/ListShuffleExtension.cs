namespace Home.AutoPlayer.Helpers;


using System;
using System.Collections.Generic;

internal static class ListShuffleExtensions {
    // In-place shuffle
    public static void Shuffle<T>(this IList<T> list, Random? rng = null) {
        rng ??= Random.Shared;

        for (int i = list.Count - 1; i > 0; i--) {
            int j = rng.Next(i + 1); // 0..i
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // Non-mutating (returns a shuffled copy)
    public static List<T> Shuffled<T>(this IEnumerable<T> source, Random? rng = null) {
        var copy = source is List<T> l ? new List<T>(l) : new List<T>(source);
        copy.Shuffle(rng);
        return copy;
    }
}
