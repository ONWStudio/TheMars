using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false), Conditional("UNITY_EDITOR")]
    public sealed class OnValueChangedByValueAttribute : PropertyAttribute
    {
        public string MethodName { get; }

        public OnValueChangedByValueAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
