﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheraBytes.BetterUi.Editor.ThirdParty;
using UnityEditor;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    [CustomPropertyDrawer(typeof(List<Transitions>))]
    public class TransitionCollectionDrawer : PropertyDrawer
    {
        private string[] stateNames;
        private SerializedProperty listProperty;
        private ReorderableListControl listEditor;
        private IReorderableListAdaptor adaptor;

        public TransitionCollectionDrawer() { }
        public TransitionCollectionDrawer(Type containerObjectType, 
            string fieldName = "betterTransitions",
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo fieldInfo = containerObjectType.GetField(fieldName, bindingFlags);
            TryReadStateNames(fieldInfo);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            TryReadStateNames(base.fieldInfo);
            Draw(() => property);
        }

        public void Draw(Func<SerializedProperty> getProperty)
        {
            if(this.listProperty == null)
            {
                this.listProperty = getProperty();
            }

            if (adaptor == null)
            {
                adaptor = new SerializedPropertyAdaptor(listProperty);
            }

            if(stateNames == null)
            {
                string msg = string.Format(
                        "No [TransitionStates(...)] or [DefaultTransitionStates] attribute defined for field <b>{0}</b>",
                        listProperty.displayName);

                EditorGUILayout.HelpBox(msg, MessageType.Error);
                return;
            }

            if (listEditor == null)
            {
                listEditor = new ReorderableListControl(ReorderableListFlags.ShowSizeField);
                listEditor.ItemInserted += ItemInserted;
            }

            ReorderableListGUI.Title(listProperty.displayName);
            listEditor.Draw(adaptor);
            listProperty.serializedObject.ApplyModifiedProperties();
        }

        private bool TryReadStateNames(FieldInfo fieldInfo)
        {
            if (stateNames != null)
                return true;

            TransitionStatesAttribute attribute = fieldInfo.GetCustomAttributes(typeof(TransitionStatesAttribute), true)
                    .FirstOrDefault() as TransitionStatesAttribute;

            if (attribute == null)
                return false;

            stateNames = attribute.States;
            return true;
        }

        private void ItemInserted(object sender, ItemInsertedEventArgs args)
        {
            SerializedProperty p = listProperty.GetArrayElementAtIndex(args.ItemIndex);
            SerializedProperty namesProp = p.FindPropertyRelative("stateNames");

            // unity copies the previous content.
            // so we need to fill the array only the first time.
            if (namesProp.arraySize >= stateNames.Length)
                return;

            for (int i = 0; i < stateNames.Length; i++)
            {
                namesProp.InsertArrayElementAtIndex(i);
                namesProp.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                string name = stateNames[i];
                SerializedProperty elem = namesProp.GetArrayElementAtIndex(i);
                elem.stringValue = name;
            }

            namesProp.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
