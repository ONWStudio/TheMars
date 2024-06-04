using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubClassSelectorSpace
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class SubClassSelectorAttribute : PropertyAttribute
    {
        public Type BaseType { get; }

        public SubClassSelectorAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}
