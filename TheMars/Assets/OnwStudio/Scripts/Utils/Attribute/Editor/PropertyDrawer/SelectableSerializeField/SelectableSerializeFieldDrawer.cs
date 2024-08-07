#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Onw.Attribute.Editor
{
    using GUI = UnityEngine.GUI;

    [CustomPropertyDrawer(typeof(SelectableSerializeFieldAttribute), true)]
    internal sealed class SelectableSerializeFieldDrawer : InitializablePropertyDrawer
    {
        private readonly TreeViewState _dropdownState = new();
        private ComponentDropdown _dropdown = null;
        private GUIContent _buttonContent = null;
        private bool _isMonoBehaviour = false;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _isMonoBehaviour = property.serializedObject.targetObject is MonoBehaviour;

            if (_isMonoBehaviour)
            {
                _dropdown = new(_dropdownState, (property.serializedObject.targetObject as Component).gameObject, fieldInfo.FieldType, go =>
                {
                    if (fieldInfo.FieldType == typeof(GameObject))
                    {
                        property.objectReferenceValue = go;
                    }
                    else
                    {
                        property.objectReferenceValue = go.GetComponent(fieldInfo.FieldType);
                    }

                    property.serializedObject.ApplyModifiedProperties();
                });

                _buttonContent = new(EditorGUIUtility.IconContent("icon dropdown").image);
            }
        }

        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_isMonoBehaviour)
            {
                EditorGUI.HelpBox(position, "this object is not MonoBehaviour!", MessageType.Error);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            Rect fieldRect = position;
            GUIContent content = label;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Rect labelRect = new(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
                Rect buttonRect = new(labelRect.xMax, position.y, 18, position.height);

                fieldRect = new Rect(
                    buttonRect.xMax + 2,
                    position.y,
                    position.width - (buttonRect.xMax - position.x),
                    position.height);

                content = GUIContent.none;
                EditorGUI.LabelField(labelRect, label);

                if (GUI.Button(buttonRect, _buttonContent))
                {
                    _dropdown.Show(buttonRect);
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use SelectableComponent With Component or GameObject");
            }

            GUI.enabled = false;
            EditorGUI.PropertyField(fieldRect, property, content);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif