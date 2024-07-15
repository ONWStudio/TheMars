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
        /// <summary>
        /// .. Key의 Label
        /// /// </summary>
        public readonly string KeyName;
        /// <summary>
        /// .. Value의 Label
        /// </summary>
        public readonly string ValueName;

        public SerializedDictionaryAttribute(string keyName = null, string valueName = null)
        {
            KeyName = keyName;
            ValueName = valueName;
        }
    }
}