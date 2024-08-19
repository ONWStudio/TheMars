using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace SerializeReferenceDropdown.Editor
{
    public class GenericTypeCreateWindow : EditorWindow
    {
        private const int InvalidIndex = -1;

        private SerializedProperty serializedProperty;
        private Type inputGenericType;
        private Action<Type> onSelectNewGenericType;

        private IReadOnlyList<Type> specifiedTypesFromProperty;

        private List<IReadOnlyList<Type>> typesForParameters;
        private List<IReadOnlyList<string>> typeNamesForParameters;
        private int[] selectedIndexes;

        private IReadOnlyList<Button> parameterTypeButtons;
        private IReadOnlyList<Toggle> makeArrayTypeToggles;
        private Button generateGenericTypeButton;

        public static void ShowCreateTypeMenu(SerializedProperty property, Rect propertyRect, Type genericType,
            Action<Type> onSelectedConcreteType)
        {
            GenericTypeCreateWindow window = GetWindow<GenericTypeCreateWindow>();
            window.titleContent = new GUIContent("SRD Generic Type Create");
            window.Show();
            window.InitWindow(property, genericType, onSelectedConcreteType);
            window.CreateElements();

            Rect currentRect = window.position;
            Vector2 windowPos = GUIUtility.GUIToScreenPoint(propertyRect.position);
            currentRect.position = windowPos;
            window.position = currentRect;
        }

        private void InitWindow(SerializedProperty property, Type genericType, Action<Type> onSelectedConcreteType)
        {
            inputGenericType = genericType;
            onSelectNewGenericType = onSelectedConcreteType;
            serializedProperty = property;

            Type[] genericParams = inputGenericType.GetGenericArguments();
            selectedIndexes = new int[genericParams.Length];
            FillTypesAndNames();
            FillSpecifiedTypesFromProperty();
        }

        private string GetTypeName(Type type) => type.FullName;

        private void FillTypesAndNames()
        {
            Type[] genericParams = inputGenericType.GetGenericArguments();
            typesForParameters = new List<IReadOnlyList<Type>>();
            typeNamesForParameters = new List<IReadOnlyList<string>>();
            for (int i = 0; i < selectedIndexes.Length; i++)
            {
                selectedIndexes[i] = InvalidIndex;
                Type genericParam = genericParams[i];
                IReadOnlyList<Type> targetTypes;
                IReadOnlyList<Type> systemObjectTypes = TypeUtils.GetAllSystemObjectTypes();
                if (genericParam.GetInterfaces().Length == 0)
                {
                    targetTypes = systemObjectTypes;
                }
                else
                {
                    TypeCache.TypeCollection typesCollection = TypeCache.GetTypesDerivedFrom(genericParam);
                    targetTypes = typesCollection.Where(t => systemObjectTypes.Contains(t)).ToArray();
                }

                typesForParameters.Add(targetTypes);
                string[] names = targetTypes.Select(GetTypeName).ToArray();
                typeNamesForParameters.Add(names);
            }
        }

        private void FillSpecifiedTypesFromProperty()
        {
            Type propertyType = TypeUtils.ExtractTypeFromString(serializedProperty.managedReferenceFieldTypename);
            if (propertyType.IsGenericType == false || propertyType.IsInterface == false)
            {
                return;
            }

            Type[] genericInterfaces = inputGenericType.GetInterfaces();
            int genericInterfaceIndex = Array.FindIndex(genericInterfaces, IsSameGenericInterface);
            Type genericInterface = genericInterfaces[genericInterfaceIndex];
            Type[] genericInterfaceArgs = genericInterface.GetGenericArguments();

            Type[] propertyGenericArgs = propertyType.GetGenericArguments();
            Type[] genericTypeArgs = inputGenericType.GetGenericArguments();
            Type[] specifiedTypes = new Type[genericTypeArgs.Length];
            for (int i = 0; i < genericInterfaceArgs.Length; i++)
            {
                Type genericArg = genericInterfaceArgs[i];
                Type specifiedType = propertyGenericArgs[i];
                int genericArgIndex = Array.FindIndex(genericTypeArgs, Match);
                specifiedTypes[genericArgIndex] = specifiedType;

                bool Match(Type type)
                {
                    return type.Name == genericArg.Name;
                }
            }

            specifiedTypesFromProperty = specifiedTypes;

            bool IsSameGenericInterface(Type type)
            {
                return type.IsGenericType && propertyType.GetGenericTypeDefinition() == type.GetGenericTypeDefinition();
            }
        }


        private void OnGUI()
        {
            if (IsDisposedProperty())
            {
                Close();
            }

            bool IsDisposedProperty()
            {
                if (serializedProperty == null)
                {
                    return true;
                }

                Type propertyType = serializedProperty.GetType();
                PropertyInfo isValidField = propertyType.GetProperty("isValid", BindingFlags.NonPublic | BindingFlags.Instance);
                object isValidValue = isValidField?.GetValue(serializedProperty);
                return isValidValue != null && (bool)isValidValue == false;
            }
        }


        private void CreateElements()
        {
            rootVisualElement.Clear();
            CreateParameterButtons();
            CreateGenerateGenericTypeButton();
        }

        private void CreateParameterButtons()
        {
            List<Button> parameterButtons = new List<Button>();
            List<Toggle> arrayToggles = new List<Toggle>();
            parameterTypeButtons = parameterButtons;
            makeArrayTypeToggles = arrayToggles;

            Type[] genericParams = inputGenericType.GetGenericArguments();
            for (int i = 0; i < genericParams.Length; i++)
            {
                int index = i;
                Type currentParam = genericParams[i];
                string paramName = $"[{i}] {currentParam.Name}";
                Button button = new Button();

                button.clickable.clicked += () => ShowTypesForParamIndex(index, button);

                TextElement parameterTypeLabel = new TextElement();
                parameterTypeLabel.text = paramName;

                Toggle makeArrayToggle = new Toggle("Make Array Type");

                Box group = new Box();
                group.style.flexDirection = FlexDirection.Row;
                group.style.alignItems = Align.Center;
                group.Add(parameterTypeLabel);
                group.Add(button);
                group.Add(makeArrayToggle);

                parameterButtons.Add(button);
                arrayToggles.Add(makeArrayToggle);

                rootVisualElement.Add(group);

                RefreshGenericParameterButton(i);
            }
        }

        private void CreateGenerateGenericTypeButton()
        {
            generateGenericTypeButton = new Button();
            generateGenericTypeButton.text = "Generate";
            generateGenericTypeButton.clickable.clicked += GenerateGenericType;
            rootVisualElement.Add(generateGenericTypeButton);

            RefreshGenerateGenericButton();
        }

        private void GenerateGenericType()
        {
            Type[] parameterTypes = new Type[selectedIndexes.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                Type type;
                if (specifiedTypesFromProperty[i] != null)
                {
                    type = specifiedTypesFromProperty[i];
                }
                else
                {
                    int typeIndex = selectedIndexes[i];
                    type = typesForParameters[i][typeIndex];
                    if (type.IsArray == false && makeArrayTypeToggles[i].value)
                    {
                        type = type.MakeArrayType();
                    }
                }

                parameterTypes[i] = type;
            }

            Type newGenericType = inputGenericType.MakeGenericType(parameterTypes);
            try
            {
                onSelectNewGenericType.Invoke(newGenericType);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            Close();
        }

        private void ShowTypesForParamIndex(int genericParamIndex, Button selectedButton)
        {
            IReadOnlyList<string> currentTypeNames = typeNamesForParameters[genericParamIndex];

            AdvancedDropdown dropdown = new AdvancedDropdown(new AdvancedDropdownState(), currentTypeNames,
                ApplySelectedTypeIndex);

            Rect buttonRect = new Rect(selectedButton.transform.position, selectedButton.transform.scale);
            dropdown.Show(buttonRect);

            void ApplySelectedTypeIndex(int selectedTypeIndex)
            {
                selectedIndexes[genericParamIndex] = selectedTypeIndex;
                RefreshGenericParameterButton(genericParamIndex);
                RefreshGenerateGenericButton();
            }
        }

        private void RefreshGenericParameterButton(int parameterIndex)
        {
            Button button = parameterTypeButtons[parameterIndex];
            Type specifiedType = specifiedTypesFromProperty[parameterIndex];
            if (specifiedType != null)
            {
                button.text = GetTypeName(specifiedType);
                button.SetEnabled(false);
                makeArrayTypeToggles[parameterIndex].SetEnabled(false);
                return;
            }

            int selectedIndex = selectedIndexes[parameterIndex];
            string buttonText = selectedIndex == InvalidIndex
                ? "Select Type"
                : typeNamesForParameters[parameterIndex][selectedIndex];
            button.text = buttonText;
        }


        private void RefreshGenerateGenericButton()
        {
            int specifiedTypesCount = specifiedTypesFromProperty.Count(t => t != null);
            int selectedIndexesCount = selectedIndexes.Count(t => t != InvalidIndex);
            bool isSelectedAllTypes = (specifiedTypesCount + selectedIndexesCount) == selectedIndexes.Length;
            generateGenericTypeButton.SetEnabled(isSelectedAllTypes);
        }
    }
}