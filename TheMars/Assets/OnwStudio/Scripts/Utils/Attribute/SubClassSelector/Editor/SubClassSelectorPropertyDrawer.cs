#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SubClassSelectorSpace;
using static EditorTool.EditorTool;

namespace SubClassSelectorEditor
{
    [CustomPropertyDrawer(typeof(SubClassSelectorAttribute), true)]
    public sealed class SubClassSelectorPropertyDrawer : PropertyDrawer
    {
        private readonly List<Type> _subClasses = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SubClassSelectorAttribute subClassSelector = attribute as SubClassSelectorAttribute;

            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                EditorGUI.LabelField(position, label.text, "Use [SubclassSelector] with TypeReferecne.");
                return;
            }

            Type baseType = subClassSelector.BaseType;

            if (_subClasses.Count <= 0)
            {
                _subClasses.AddRange(ReflectionHelper
                    .GetChildClassesFromBaseType(baseType));
            }

            string[] subClassNames = _subClasses
                .Select(type => type.Name)
                .ToArray();

            string currentSubClass = property.managedReferenceValue?.GetType().Name;

            int selectedIndex = Array
                .FindIndex(subClassNames, className => className == currentSubClass);

            if (selectedIndex == -1)
            {
                selectedIndex = 0;
            }

            selectedIndex = EditorGUI.Popup(
                position,
                label.text,
                selectedIndex,
                subClassNames);

            if (selectedIndex >= 0 && selectedIndex < _subClasses.Count)
            {
                Type selectedType = _subClasses[selectedIndex];

                if (property.managedReferenceValue == null || property.managedReferenceValue.GetType() != selectedType)
                {
                    property.managedReferenceValue = Activator
                        .CreateInstance(selectedType);
                }

                EditorGUILayout.LabelField("Options");
                ActionEditorVertical(() =>
                {
                    SerializedProperty iterator = property.Copy();
                    SerializedProperty endProperty = property.GetEndProperty();

                    iterator.NextVisible(true);
                    while (!SerializedProperty.EqualContents(iterator, endProperty))
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                        iterator.NextVisible(false);
                    }
                }, GUI.skin.box);
            }

            property
                .serializedObject
                .ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif
