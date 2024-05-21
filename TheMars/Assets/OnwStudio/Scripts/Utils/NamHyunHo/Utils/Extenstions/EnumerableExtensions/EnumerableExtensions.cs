using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// .. Linq 커스텀 기능
public static class EnumerableExtensions
{
    public static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> defaultCorrection)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (defaultCorrection == null)
            throw new ArgumentNullException(nameof(defaultCorrection));

        return source.Any() ? source : defaultCorrection;
    }

    public static IEnumerable<T> RemoveElements<T>(this List<T> source, Func<T> predicate)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        _ = source.Remove(predicate.Invoke());
        return source;
    }

    public static void ForEach<T>(this IList<T> source, Action<T> action)
    {
        for (int i = 0; i < source.Count; i++) action.Invoke(source[i]);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source) action.Invoke(element);
    }

    public static void ForEach<T>(this IEnumerable source, Action<T> action)
    {
        foreach (T element in source) action.Invoke(element);
    }
}
