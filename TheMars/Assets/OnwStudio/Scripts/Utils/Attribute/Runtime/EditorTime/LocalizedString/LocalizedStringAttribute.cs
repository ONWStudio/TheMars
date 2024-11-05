using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field), Conditional("UNITY_EDITOR")]
    public class LocalizedStringAttribute : PropertyAttribute
    {
        public string TableName { get; }
        public string EntryKey { get; }

        public LocalizedStringAttribute(string tableName = null, string entryKey = null)
        {
            TableName = tableName;
            EntryKey = entryKey;
        }

    }
}