#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Onw.Editor;

namespace Onw.Attribute.Editor
{
    using GUI = UnityEngine.GUI;

    [CustomPropertyDrawer(typeof(SelectableSerializeFieldAttribute), true)]
    internal sealed class SelectableSerializeFieldDrawer : PropertyDrawer
    {
        public readonly struct SelectableFieldPropState
        {
            public ComponentDropdown Dropdown { get; }
            public bool IsMonoBehaviour { get; }

            public SelectableFieldPropState(ComponentDropdown dropdown, bool isMonoBehaviour)
            {
                Dropdown = dropdown;
                IsMonoBehaviour = isMonoBehaviour;
            }
        }
        
        private readonly TreeViewState _dropdownState = new();
        private GUIContent _buttonContent = null;

        private readonly Dictionary<int, SelectableFieldPropState> _cachedSelectableFieldPropStates = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _buttonContent ??= new(EditorGUIUtility.IconContent("icon dropdown").image);

            int hash = property.GetHashCode();
            if (!_cachedSelectableFieldPropStates.TryGetValue(hash, out SelectableFieldPropState state))
            {
                if (property.serializedObject.targetObject is MonoBehaviour monoBehaviour)
                {
                    ComponentDropdown dropdown = new(_dropdownState, monoBehaviour.gameObject, fieldInfo.FieldType, go =>
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

                    state = new(dropdown, true);
                }
                else
                {
                    state = new(null, false);
                }
                
                _cachedSelectableFieldPropStates.Add(hash, state);
            }
            
            if (!state.IsMonoBehaviour)
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

                fieldRect = new(
                    buttonRect.xMax + 2,
                    position.y,
                    position.width - (buttonRect.xMax - position.x),
                    position.height);

                content = GUIContent.none;
                EditorGUI.LabelField(labelRect, label);

                if (GUI.Button(buttonRect, _buttonContent))
                {
                    state.Dropdown.Show(buttonRect);
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