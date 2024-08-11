using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Helper
{
    public static class ListHelper 
    {
        public static void AddByListCapacity<T>(ref int capacityValue, List<T> list)
        {
            if (list is null) return;

            capacityValue += list.Count;
        }
    }
}
