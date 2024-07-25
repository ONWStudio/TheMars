using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, Inherited = true, AllowMultiple = false), Conditional("UNITY_EDITOR")]
    public sealed class DisplayAsAttribute : PropertyAttribute
    {
        public string DisplayName { get; }

        public DisplayAsAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
