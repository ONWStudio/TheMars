using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Attribute
{
    using Attribute = System.Attribute;
    /// <summary>
    /// .. 에디터에서 특정 프로퍼티의 값이 변경되면 메서드를 호출하게 하는 어트리뷰트 입니다
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class OnValueChangedByMethodAttribute : PropertyAttribute
    {
        public string FieldName { get; }

        public OnValueChangedByMethodAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
