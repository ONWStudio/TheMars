using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    public static class ListExtentions
    {
        public static void AddElements<T>(this List<T> values, params T[] element) => values.AddRange(element);
    }
}