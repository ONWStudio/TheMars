using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field), Conditional("UNITY_EDITOR")]
    public class OnwMaxAttribute : PropertyAttribute
    {
        public int Max { get; }

        public OnwMaxAttribute(int max)
        {
            Max = max;
        }
    }
}