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
    internal sealed class InitializeRequireComponentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;

            Type requireComponentType = property.GetPropertyType();
            bool isValidate = typeof(MonoBehaviour).IsSubclassOf(requireComponentType) && 
                              typeof(Component).IsSubclassOf(requireComponentType);
            
            if (!isValidate && !property.IsNestedAttribute<SerializeField>())
            {
                Debug.LogWarning("Not MonoBehaviour or Component! and check in Contain SerializeField Attribute");
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
