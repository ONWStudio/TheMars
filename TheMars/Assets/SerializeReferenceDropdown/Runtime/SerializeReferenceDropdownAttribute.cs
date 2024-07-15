using System;
using System.Diagnostics;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field), Conditional("UNITY_EDITOR")]
public class SerializeReferenceDropdownAttribute : PropertyAttribute {}