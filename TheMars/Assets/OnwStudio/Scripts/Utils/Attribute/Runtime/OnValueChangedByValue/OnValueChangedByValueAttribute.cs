using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class OnValueChangedByValueAttribute : PropertyAttribute
    {
        public string MethodName { get; }

        public OnValueChangedByValueAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
