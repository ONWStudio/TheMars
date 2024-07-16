#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Onw.Editor
{
    using Attribute = System.Attribute;

    public static class EditorReflectionHelper
    {
        /// <summary>
        /// .. 자동구현 프로퍼티의 백킹 필드의 직렬화 된 이름을 가져옵니다.
        /// </summary>
        /// <param name="propertyName">
        /// 프로퍼티 이름
        /// </param>
        /// <returns></returns>
        public static string GetBackingFieldName(string propertyName) => $"<{propertyName}>k__BackingField";

        public static IEnumerable<Attribute> GetAttributes(object targetField)
        {
            if (targetField is null) yield break;

            foreach (Attribute attribute in targetField.GetType().GetCustomAttributes<Attribute>())
            {
                yield return attribute;
            }
        }

        public static IEnumerable<Attribute> GetAttributes(object targetObject, string propertyPath)
        {
            if (targetObject is null) yield break;

            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            if (field is not null)
            {
                foreach (Attribute attribute in field.GetCustomAttributes<Attribute>())
                {
                    yield return attribute;
                }
            }
        }

        public static FieldInfo GetFieldInfo(object targetObject, string propertyPath)
        {
            if (targetObject is null || string.IsNullOrEmpty(propertyPath)) return null;

            return targetObject
                .GetType()
                .GetField(propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        public static Type GetPropertyType(object targetObject, string propertyPath)
        {
            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(UnityEngine.Object))
            {
                var genericArgs = field.FieldType.GetGenericArguments();
                if (genericArgs.Length > 0)
                {
                    return genericArgs[0];
                }
            }

            return field.FieldType;
        }

        public static bool IsNestedAttribute<AttributeType>(object targetObject) where AttributeType : Attribute
        {
            return targetObject is not null && targetObject.GetType().GetCustomAttribute<AttributeType>() is not null;
        }

        public static bool IsNestedAttribute<AttributeType>(object targetObject, string propertyPath) where AttributeType : Attribute
        {
            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            return field is not null && field.GetCustomAttribute<AttributeType>() is not null;
        }
    }
}
#endif