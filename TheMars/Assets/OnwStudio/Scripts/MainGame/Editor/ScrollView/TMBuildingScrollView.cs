#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using Unity.EditorCoroutines.Editor;
using Onw.Editor.Window;
using Onw.Editor.Extensions;
using Onw.Extensions;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Building;

namespace TM.Editor
{
    internal sealed class TMBuildingScrollView : ScriptableObjectScrollView
    {
        private readonly List<EditorCoroutine> _buildingNameObservers = new();

        protected override VisualElement CreateHeader()
        {
            VisualElement header = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    borderBottomColor = Color.black,
                    borderBottomWidth = 1f,
                    height = 25f,
                    maxHeight = 25f
                },
            };

            Button addButton = createHeaderButton(()
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMBuildingData>("Assets/OnwStudio/ScriptableObject/Buildings", $"Building_No.{Guid.NewGuid()}")));
            addButton.text = "추가";
            addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#1B5914", out Color addColor) ? addColor : Color.green;
            addButton.SetChangedColorButtonEvent();
            header.Add(addButton);

            return header;

            static Button createHeaderButton(Action clickedEvent)
            {
                Button button = new(clickedEvent)
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        flexGrow = 1,
                        height = 20f,
                        maxHeight = 20f,
                        minHeight = 20f
                    },
                };

                return button;
            }
        }

        protected override ScriptableObjectButton CreateButton(ScriptableObject so)
        {
            ScriptableObjectButton button = base.CreateButton(so);
            TMBuildingData building = (button.ScriptableObject as TMBuildingData)!;

            button.text = "";

            Label field = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };

            if (building.TryGetFieldByName(
                "_localizedBuildingName",
                BindingFlags.NonPublic | BindingFlags.Instance,
                out LocalizedString localizedBuildingName))
            {
                _buildingNameObservers.Add(localizedBuildingName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
                if (localizedBuildingName.IsEmpty)
                {
                    button.name = field.text = building.name;
                }
                else
                {
                    button.name = field.text = localizedBuildingName.GetLocalizedString();
                }

                void onChangedString(string buildingName)
                {
                    button.name = field.text = string.IsNullOrEmpty(buildingName) ? building.name : buildingName;
                }
            }
            else
            {
                button.name = field.text = building.name;
            }

            button.Add(field);

            Button removeButton = new(() =>
            {
                if (_selectedObject?.ScriptableObject == so)
                {
                    _selectedObject = null;
                }

                RemoveSo(so);
                UnityEngine.Object.DestroyImmediate(so, true);
            })
            {
                style =
                {
                    position = Position.Absolute,
                    right = new Length(2, LengthUnit.Percent),
                    top = new Length(50, LengthUnit.Percent),
                    translate = new Translate(0, Length.Percent(-50)),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = ColorUtility.TryParseHtmlString("#4C1212", out Color removeColor) ? removeColor : Color.red,
                    height = 20,
                    width = 20,
                },
                text = "X",
            };

            removeButton.SetChangedColorButtonEvent();
            button.Add(removeButton);
            return button;
        }

        internal override void OnDisable()
        {
            _buildingNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif
