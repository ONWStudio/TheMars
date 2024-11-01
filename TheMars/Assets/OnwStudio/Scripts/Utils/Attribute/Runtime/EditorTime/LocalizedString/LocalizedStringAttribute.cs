using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    [AttributeUsage(AttributeTargets.Field), Conditional("UNITY_EDITOR")]
    public class LocalizedStringAttribute : PropertyAttribute
    {
        public string TableName { get; }
        public string EntryKey { get; }

        public LocalizedStringAttribute()
        {
            TableName = null;
            EntryKey = null;
        }
        
        public LocalizedStringAttribute(string tableName)
        {
            TableName = tableName;
            EntryKey = null;
        }

        public LocalizedStringAttribute(string tableName, string entryKey)
        {
            TableName = tableName;
            EntryKey = entryKey;
        }
    }
}