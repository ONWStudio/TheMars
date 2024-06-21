#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OnwAttributeExtensions;

namespace OnValueChangedAttributeEditor
{
    [CustomPropertyDrawer(typeof(OnValueChangedByValueAttribute))]
    internal sealed class OnValueChangedByValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying) return;

            OnValueChangedByValueAttribute onValueChangedAttribute = attribute as OnValueChangedByValueAttribute;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, true);

            if (EditorGUI.EndChangeCheck())
            {
                object target = property.serializedObject.targetObject;

                BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

                MethodInfo method = target.GetType().GetMethod(onValueChangedAttribute.MethodName, flags);

                if (method is not null)
                {
                    method.Invoke(target, null);
                }
                else
                {
                    Debug.LogWarning($"Method {onValueChangedAttribute.MethodName} not found on {target.GetType().Name}");
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
