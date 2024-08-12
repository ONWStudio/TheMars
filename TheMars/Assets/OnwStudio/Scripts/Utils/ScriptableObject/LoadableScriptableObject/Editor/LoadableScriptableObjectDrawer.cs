#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Onw.Editor;

namespace Onw.ScriptableObjects.Editor
{
    [CustomPropertyDrawer(typeof(LoadableScriptableObject<>), true)]
    internal sealed class LoadableScriptableObjectDrawer : InitializablePropertyDrawer
    {
        private ReorderableList _reorderableList = null;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty listProperty = property.FindPropertyRelative("_scriptableObjects");

            _reorderableList = new(property.serializedObject, listProperty, true, true, false, false)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, label),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index), GUIContent.none);
                    GUI.enabled = true;
                },
                elementHeight = EditorGUIUtility.singleLineHeight
            };

            Type listFieldType = fieldInfo.FieldType.GetElementType() ?? fieldInfo.FieldType.GetGenericArguments()[0];

            List<ScriptableObject> scriptableObjects = AssetDatabase
                .FindAssets($"t:{listFieldType.Name}")
                .Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), listFieldType))
                .OfType<ScriptableObject>()
                .ToList();

            listProperty.ClearArray();
            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                listProperty.InsertArrayElementAtIndex(i);
                listProperty.GetArrayElementAtIndex(i).objectReferenceValue = scriptableObjects[i];
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _reorderableList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _reorderableList?.GetHeight() ?? EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif