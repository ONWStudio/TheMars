using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [Conditional("UNITY_EDITOR")]
    public class SerializedDictionaryAttribute : Attribute
    {
        public readonly string KeyName;
        public readonly string ValueName;
        public readonly bool IsReadOnlyKey;
        public readonly bool IsReadOnlyValue;
        public readonly bool IsLocked;

        public SerializedDictionaryAttribute(string keyName = null, string valueName = null, bool isReadOnlyKey = false, bool isReadOnlyValue = false, bool isLooked = false)
        {
            KeyName = keyName;
            ValueName = valueName;
            IsReadOnlyKey = isReadOnlyKey;
            IsReadOnlyValue = isReadOnlyValue;
            IsLocked = isLooked;
        }
    }
}