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
        /// <summary>
        /// .. 직렬화 딕셔너리의 키가 읽기 전용인가? (인스펙터만 적용)
        /// </summary>
        public readonly bool IsReadOnlyKey;
        /// <summary>
        /// .. 직렬화 딕셔너리의 밸류가 읽기 전용인가? (인스펙터만 적용) 
        /// </summary>
        public readonly bool IsReadOnlyValue;
        /// <summary>
        /// .. 추가 삽입이 가능한가? (인스펙터만 적용) 
        /// </summary>
        public readonly bool IsLocked;

        public SerializedDictionaryAttribute(string keyName = null, string valueName = null, bool isReadOnlyKey = false, bool isReadOnlyValue = false, bool isLocked = false)
        {
            KeyName = keyName;
            ValueName = valueName;
            IsReadOnlyKey = isReadOnlyKey;
            IsReadOnlyValue = isReadOnlyValue;
            IsLocked = isLocked;
        }
    }
}