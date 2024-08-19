using System;
using UnityEditor;
using UnityEngine;

namespace SerializeReferenceDropdown.Editor
{
    [InitializeOnLoad]
    public class CopyPasteContextMenu
    {
        private static (string json, Type type) lastObject;

        static CopyPasteContextMenu()
        {
            EditorApplication.contextualPropertyMenu += ShowSerializeReferenceCopyPasteContextMenu;
        }

        private static void ShowSerializeReferenceCopyPasteContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                SerializedProperty copyProperty = property.Copy();
                menu.AddItem(new GUIContent("Copy Serialize Reference"), false,
                    (_) => { CopyReferenceValue(copyProperty); }, null);
                GUIContent pasteContent = new GUIContent("Paste Serialize Reference");
                menu.AddItem(pasteContent, false, (_) => PasteReferenceValue(copyProperty),
                    null);
                if (property.IsArrayElement())
                {
                    GUIContent duplicateContent = new GUIContent("Duplicate Serialize Reference Array Element");
                    menu.AddItem(duplicateContent, false, (_) => DuplicateSerializeReferenceArrayElement(copyProperty),
                        null);
                }
            }
        }

        private static void CopyReferenceValue(SerializedProperty property)
        {
            object refValue = GetReferenceToValueFromSerializerPropertyReference(property);
            lastObject.json = JsonUtility.ToJson(refValue);
            lastObject.type = refValue?.GetType();
        }

        private static void PasteReferenceValue(SerializedProperty property)
        {
            try
            {
                if (lastObject.type != null)
                {
                    object pasteObj = JsonUtility.FromJson(lastObject.json, lastObject.type);
                    property.managedReferenceValue = pasteObj;
                }
                else
                {
                    property.managedReferenceValue = null;
                }

                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                PropertyDrawer.UpdateDropdownCallback?.Invoke();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private static void DuplicateSerializeReferenceArrayElement(SerializedProperty property)
        {
            object sourceElement = GetReferenceToValueFromSerializerPropertyReference(property);
            SerializedProperty arrayProperty = TypeUtils.GetArrayPropertyFromArrayElement(property);
            int newElementIndex = arrayProperty.arraySize;
            arrayProperty.arraySize = newElementIndex + 1;

            if (sourceElement != null)
            {
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                
                string json = JsonUtility.ToJson(sourceElement);
                object newObj = JsonUtility.FromJson(json, sourceElement.GetType());
                SerializedProperty newElementProperty = arrayProperty.GetArrayElementAtIndex(newElementIndex);
                newElementProperty.managedReferenceValue = newObj;
            }

            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private static object GetReferenceToValueFromSerializerPropertyReference(SerializedProperty property)
        {
#if UNITY_2021_2_OR_NEWER
            return property.managedReferenceValue;
#else
            return property.GetTarget();
#endif
        }
    }
}