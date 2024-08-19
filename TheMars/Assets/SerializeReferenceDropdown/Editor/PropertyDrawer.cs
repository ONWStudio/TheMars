using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SerializeReferenceDropdown.Editor
{
    [CustomPropertyDrawer(typeof(SerializeReferenceDropdownAttribute))]
    public class PropertyDrawer : UnityEditor.PropertyDrawer
    {
        private const string NullName = "null";
        private List<Type> assignableTypes;
        private Rect propertyRect;

        public static Action UpdateDropdownCallback;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            propertyRect = rect;
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                DrawIMGUITypeDropdown(rect, property, label);
            }
            else
            {
                EditorGUI.PropertyField(rect, property, label, true);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                DrawUIToolkitTypeDropdown(root, property);
            }
            else
            {
                root.Add(new PropertyField(property));
            }

            return root;
        }

        private void DrawUIToolkitTypeDropdown(VisualElement root, SerializedProperty property)
        {
            string uiToolkitLayoutPath =
                "Packages/com.alexeytaranov.serializereferencedropdown/Editor/Layouts/SerializeReferenceDropdown.uxml";
            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uiToolkitLayoutPath);
            root.Add(visualTreeAsset.Instantiate());
            PropertyField propertyField = root.Q<PropertyField>();
            Button selectTypeButton = root.Q<Button>();
            selectTypeButton.clickable.clicked += ShowDropdown;
            assignableTypes ??= GetAssignableTypes(property);
            UpdateDropdown();
            UpdateDropdownCallback = UpdateDropdown;

            void ShowDropdown()
            {
                AdvancedDropdown dropdown = new AdvancedDropdown(new AdvancedDropdownState(),
                    assignableTypes.Select(GetTypeName), index => { WriteNewInstanceByIndexType(index, property); });
                Matrix4x4 buttonMatrix = selectTypeButton.worldTransform;
                Vector3 position = new Vector3(buttonMatrix.m03, buttonMatrix.m13, buttonMatrix.m23);
                Rect buttonRect = new Rect(position, selectTypeButton.contentRect.size);
                dropdown.Show(buttonRect);
            }

            void UpdateDropdown()
            {
                propertyField.BindProperty(property);
                Type selectedType = TypeUtils.ExtractTypeFromString(property.managedReferenceFullTypename);
                string selectedTypeName = GetTypeName(selectedType);
                selectTypeButton.text = selectedTypeName;
            }
        }

        private void DrawIMGUITypeDropdown(Rect rect, SerializedProperty property, GUIContent label)
        {
            assignableTypes ??= GetAssignableTypes(property);
            Type referenceType = TypeUtils.ExtractTypeFromString(property.managedReferenceFullTypename);

            Rect dropdownRect = GetDropdownIMGUIRect(rect);

            EditorGUI.EndDisabledGroup();

            GUIContent dropdownTypeContent = new GUIContent(
                text: GetTypeName(referenceType),
                tooltip: GetTypeTooltip(referenceType));
            if (EditorGUI.DropdownButton(dropdownRect, dropdownTypeContent, FocusType.Keyboard))
            {
                AdvancedDropdown dropdown = new AdvancedDropdown(new AdvancedDropdownState(),
                    assignableTypes.Select(GetTypeName),
                    index => WriteNewInstanceByIndexType(index, property));
                dropdown.Show(dropdownRect);
            }

            EditorGUI.PropertyField(rect, property, label, true);

            static Rect GetDropdownIMGUIRect(Rect mainRect)
            {
                float dropdownOffset = EditorGUIUtility.labelWidth;
                Rect rect = new(mainRect);
                rect.width -= dropdownOffset;
                rect.x += dropdownOffset;
                rect.height = EditorGUIUtility.singleLineHeight;

                return rect;
            }
        }

        private string GetTypeName(Type type)
        {
            if (type == null)
            {
                return NullName;
            }

            TypeCache.TypeCollection typesWithNames = TypeCache.GetTypesWithAttribute(typeof(SerializeReferenceDropdownNameAttribute));
            if (typesWithNames.Contains(type))
            {
                SerializeReferenceDropdownNameAttribute dropdownNameAttribute = type.GetCustomAttribute<SerializeReferenceDropdownNameAttribute>();
                return dropdownNameAttribute.Name;
            }

            if (type.IsGenericType)
            {
                IEnumerable<string> genericNames = type.GenericTypeArguments.Select(t => t.Name);
                string genericParamNames = " [" + string.Join(",", genericNames) + "]";
                string genericName = ObjectNames.NicifyVariableName(type.Name) + genericParamNames;
                return genericName;
            }

            return ObjectNames.NicifyVariableName(type.Name);
        }

        private string GetTypeTooltip(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }

            TypeCache.TypeCollection typesWithTooltip = TypeCache.GetTypesWithAttribute(typeof(TypeTooltipAttribute));
            if (typesWithTooltip.Contains(type))
            {
                TypeTooltipAttribute tooltipAttribute = type.GetCustomAttribute<TypeTooltipAttribute>();
                return tooltipAttribute.tooltip;
            }

            return String.Empty;
        }

        private List<Type> GetAssignableTypes(SerializedProperty property)
        {
            Type propertyType = TypeUtils.ExtractTypeFromString(property.managedReferenceFieldTypename);
            TypeCache.TypeCollection derivedTypes = TypeCache.GetTypesDerivedFrom(propertyType);
            List<Type> nonUnityTypes = derivedTypes.Where(IsAssignableNonUnityType).ToList();
            if (!propertyType.IsAbstract && !propertyType.IsInterface)
            {
                nonUnityTypes.Add(propertyType);
            }
            nonUnityTypes.Insert(0, null);
            if (propertyType.IsGenericType && propertyType.IsInterface)
            {
                IEnumerable<Type> allTypes = TypeUtils.GetAllTypesInCurrentDomain().Where(IsAssignableNonUnityType)
                    .Where(t => t.IsGenericType);

                IEnumerable<Type> assignableGenericTypes = allTypes.Where(IsImplementedGenericInterfacesFromGenericProperty);
                nonUnityTypes.AddRange(assignableGenericTypes);
            }

            return nonUnityTypes;

            bool IsAssignableNonUnityType(Type type)
            {
                return TypeUtils.IsFinalAssignableType(type) && !type.IsSubclassOf(typeof(UnityEngine.Object));
            }

            bool IsImplementedGenericInterfacesFromGenericProperty(Type type)
            {
                IEnumerable<Type> interfaces = type.GetInterfaces().Where(t => t.IsGenericType);
                bool isImplementedInterface = interfaces.Any(t =>
                    t.GetGenericTypeDefinition() == propertyType.GetGenericTypeDefinition());
                return isImplementedInterface;
            }
        }

        private void WriteNewInstanceByIndexType(int typeIndex, SerializedProperty property)
        {
            Type newType = assignableTypes[typeIndex];
            Type propertyType = TypeUtils.ExtractTypeFromString(property.managedReferenceFieldTypename);

            if (newType?.IsGenericType == true)
            {
                Type concreteGenericType = TypeUtils.GetConcreteGenericType(propertyType, newType);
                if (concreteGenericType != null)
                {
                    CreateAndApplyNewInstanceFromType(concreteGenericType);
                }
                else
                {
                    GenericTypeCreateWindow.ShowCreateTypeMenu(property, propertyRect, newType,
                        CreateAndApplyNewInstanceFromType);
                }
            }
            else
            {
                CreateAndApplyNewInstanceFromType(newType);
            }

            void CreateAndApplyNewInstanceFromType(Type type)
            {
                object newObject;
                if (type?.GetConstructor(Type.EmptyTypes) != null)
                {
                    newObject = Activator.CreateInstance(type);
                }
                else
                {
                    newObject = type != null ? FormatterServices.GetUninitializedObject(type) : null;
                }

                ApplyValueToProperty(newObject);
            }

            void ApplyValueToProperty(object value)
            {
                property.managedReferenceValue = value;
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                UpdateDropdownCallback?.Invoke();
            }
        }
    }
}