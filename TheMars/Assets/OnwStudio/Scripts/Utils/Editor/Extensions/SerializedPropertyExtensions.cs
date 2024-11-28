#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Onw.Helper;
using UnityEditor.Graphs;
using Object = UnityEngine.Object;

namespace Onw.Editor.Extensions
{
    using Attribute = System.Attribute;

    public static class SerializedPropertyExtensions
    {
        public static Type GetPropertyType(this SerializedProperty serializedProperty)
        {
            Object targetObject = serializedProperty?.serializedObject.targetObject;
            return !targetObject ? null : EditorReflectionHelper.GetPropertyType(targetObject, serializedProperty.propertyPath);
        }

        public static Type GetParentType(this SerializedProperty property)
        {
            // SerializedObject의 targetObject (루트 객체)를 가져옴
            Type targetType = property.serializedObject.targetObject.GetType();

            // propertyPath를 '.' 기준으로 나누어 부모 필드를 추적
            string[] fieldNames = property.propertyPath.Replace(".Array.data[", "[").Split('.');

            // 마지막 필드 이전까지 추적해서 부모 필드의 타입을 얻음
            for (int i = 0; i < fieldNames.Length - 1; i++)
            {
                if (fieldNames[i].Contains("["))
                {
                    // 배열 또는 리스트 타입 추적 (배열 요소의 타입을 찾음)
                    string fieldName = fieldNames[i][..fieldNames[i].IndexOf("[", StringComparison.Ordinal)];
                    if (targetType != null)
                    {
                        FieldInfo field = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                        // 배열이나 리스트라면 요소 타입을 추적함
                        if (field != null)
                        {
                            targetType = field.FieldType.IsArray ? field.FieldType.GetElementType() : field.FieldType.GetGenericArguments()[0];
                        }
                    }
                }
                else
                {
                    // 일반 필드라면 해당 필드의 타입을 추적
                    if (targetType != null)
                    { 
                        FieldInfo field = targetType.GetField(fieldNames[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                       targetType = field?.FieldType;
                    }
                }
            }

            return targetType ?? property.serializedObject.GetType(); // 부모 필드의 타입 반환
        }
        
        // SerializedProperty의 부모 PropertyPath를 얻는 메서드
        public static string GetParentPropertyPath(this SerializedProperty property)
        {
            string path = property.propertyPath;

            // 마지막 필드를 제외하고 부모 경로 반환
            int lastDotIndex = path.LastIndexOf('.');
            return lastDotIndex == -1 ?
                // '.'가 없는 경우 루트 프로퍼티이므로 빈 문자열을 반환
                string.Empty : path[..lastDotIndex];

        }

        public static bool IsNestedAttribute<TAttributeType>(this SerializedProperty serializedProperty) where TAttributeType : Attribute
        {
            return EditorReflectionHelper.IsNestedAttribute<TAttributeType>(
                serializedProperty.serializedObject.targetObject,
                serializedProperty.propertyPath);
        }

        public static object GetPropertyValue(this SerializedProperty property) => property?.propertyType switch
        {
            SerializedPropertyType.Generic or SerializedPropertyType.ManagedReference => property.managedReferenceValue,
            SerializedPropertyType.Integer or SerializedPropertyType.LayerMask or SerializedPropertyType.Character => property.intValue,
            SerializedPropertyType.Boolean => property.boolValue,
            SerializedPropertyType.Float => property.floatValue,
            SerializedPropertyType.String => property.stringValue,
            SerializedPropertyType.Color => property.colorValue,
            SerializedPropertyType.ObjectReference => property.objectReferenceValue,
            SerializedPropertyType.Enum => property.enumNames[property.enumValueIndex],
            SerializedPropertyType.Vector2 => property.vector2Value,
            SerializedPropertyType.Vector3 => property.vector3Value,
            SerializedPropertyType.Vector4 => property.vector4Value,
            SerializedPropertyType.Rect => property.rectValue,
            SerializedPropertyType.ArraySize => property.arraySize,
            SerializedPropertyType.AnimationCurve => property.animationCurveValue,
            SerializedPropertyType.Bounds => property.boundsValue,
            SerializedPropertyType.Gradient => property.gradientValue,
            SerializedPropertyType.Quaternion => property.quaternionValue,
            SerializedPropertyType.ExposedReference => property.exposedReferenceValue,
            SerializedPropertyType.FixedBufferSize => property.fixedBufferSize,
            SerializedPropertyType.Vector2Int => property.vector2IntValue,
            SerializedPropertyType.Vector3Int => property.vector3IntValue,
            SerializedPropertyType.RectInt => property.rectIntValue,
            SerializedPropertyType.BoundsInt => property.boundsIntValue,
            SerializedPropertyType.Hash128 => property.hash128Value,
            _ => null,
        };
    }
}
#endif