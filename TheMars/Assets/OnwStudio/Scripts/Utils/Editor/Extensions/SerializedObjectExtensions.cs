#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Onw.Editor.Extensions
{
    public static class SerializedObjectExtensions
    {
        public static SerializedProperty GetProperty(this SerializedObject serializedObject, string propertyName)
        {
            return serializedObject.FindProperty(propertyName) ??
                serializedObject.FindProperty(EditorReflectionHelper.GetBackingFieldName(propertyName));
        }
    }
}
#endif
