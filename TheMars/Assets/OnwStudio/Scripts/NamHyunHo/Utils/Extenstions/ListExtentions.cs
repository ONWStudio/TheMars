using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtentions
{
    public static void AddElements<T>(this List<T> values, params T[] element) => values.AddRange(element);
}
