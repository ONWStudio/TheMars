using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnwAttributeExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DisplayAsAttribute : PropertyAttribute
    {
        public string DisplayName { get; }

        public DisplayAsAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
