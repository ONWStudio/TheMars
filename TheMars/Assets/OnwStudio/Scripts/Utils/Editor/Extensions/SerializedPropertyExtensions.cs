#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Helper;

namespace Onw.Editor.Extensions
{
    using Attribute = System.Attribute;

    public static class SerializedPropertyExtensions
    {
        public static Type GetPropertyType(this SerializedProperty serializedProperty)
        {
            var targetObject = serializedProperty.serializedObject.targetObject;
            return !targetObject ? null : EditorReflectionHelper.GetPropertyType(targetObject, serializedProperty.propertyPath);
        }

        public static bool IsNestedAttribute<AttributeType>(this SerializedProperty serializedProperty) where AttributeType : Attribute
        {
            return EditorReflectionHelper.IsNestedAttribute<AttributeType>(
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