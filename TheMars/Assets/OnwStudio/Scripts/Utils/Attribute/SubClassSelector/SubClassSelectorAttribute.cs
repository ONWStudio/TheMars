using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SubClassSelectorSpace
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false), Conditional("UNITY_EDITOR")]
    public sealed class SubClassSelectorAttribute : PropertyAttribute
    {
        public Type BaseType { get; }

        public SubClassSelectorAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}
