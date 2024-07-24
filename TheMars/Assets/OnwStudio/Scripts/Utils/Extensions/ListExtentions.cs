using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ListExtentions
    {
        public static void AddElements<T>(this List<T> values, params T[] element) => values.AddRange(element);

        public static void ForEach<T>(this IList<T> source, Action<T> action)
        {
            for (int i = 0; i < source.Count; i++) action.Invoke(source[i]);
        }
    }
}