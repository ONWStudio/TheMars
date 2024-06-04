#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CardSorterEditor
{
    [CustomPropertyDrawer(typeof(CardSorterAttribute))]
    public sealed class CardSorterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ICardSorter[] sorterInstances = ReflectionHelper
                .GetChildClassesFromType<ICardSorter>()
                .ToArray();

            ICardSorter currentSorter = fieldInfo
                .GetValue(property.serializedObject.targetObject) as ICardSorter;

            int currentIndex = Array
                .FindIndex(sorterInstances, t => t.GetType() == currentSorter?.GetType());

            string[] options = sorterInstances
                .Select(type => type.GetType().Name)
                .ToArray();

            Debug.Log("asdf");
            options.ForEach(option => Debug.Log(option));

            int selectedIndex = EditorGUI.Popup(
                position,
                label.text,
                currentIndex,
                options);

            if (selectedIndex >= 0 && selectedIndex < sorterInstances.Length)
            {
                var selectedSorter = sorterInstances[selectedIndex];
                if (currentSorter == null ||
                    currentSorter.GetType() != selectedSorter.GetType())
                {
                    fieldInfo.SetValue(
                        property.serializedObject.targetObject,
                        selectedIndex);
                }
            }
        }
    }
}
#endif
