#if UNITY_EDITOR
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Onw.Editor.Extensions;
using Onw.Editor;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InitializeRequireComponentAttribute), false)]
    internal sealed class InitializeRequireComponentDrawer : InitializablePropertyDrawer
    {
        private bool _isVaildate = false;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            Type requireComponentType = property.GetPropertyType();

            _isVaildate = (typeof(MonoBehaviour).IsSubclassOf(requireComponentType) && typeof(Component).IsSubclassOf(requireComponentType)) ||
                !property.IsNestedAttribute<SerializeField>();
        }

        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;

            if (attribute is not InitializeRequireComponentAttribute)
            {
                return;
            }

            Type requireComponentType = property.GetPropertyType();

            if (_isVaildate)
            {
                Debug.LogWarning("Not MonoBehaviour or Component! and check in Contain SerializeField Attirbute");
                return;
            }

            if (!property.objectReferenceValue && property.serializedObject.targetObject is Component component)
            {
                if (!component.TryGetComponent(requireComponentType, out Component requireComponent))
                {
                    requireComponent = component.gameObject.AddComponent(requireComponentType);
                }

                property.objectReferenceValue = requireComponent;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
