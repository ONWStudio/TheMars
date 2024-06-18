using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class ButtonAttribute : Attribute
{
    public string ButtonName { get; } = string.Empty;

    public ButtonAttribute(string buttonName)
    {
        ButtonName = buttonName;
    }
}
