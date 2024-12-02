#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using TM.Card;

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
            ScriptableObjectButton objectButton = base.CreateButton(so);
            TMBuildingData building = (objectButton.ScriptableObject as TMBuildingData)!;

            objectButton.text = "";

            Label field = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };

            LocalizedString localizedBuildingName = building.LocalizedBuildingName;

            _buildingNameObservers.Add(localizedBuildingName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
            if (localizedBuildingName.IsEmpty)
            {
                objectButton.name = field.text = building.name;
            }
            else
            {
                objectButton.name = field.text = localizedBuildingName.GetLocalizedString();
            }

            void onChangedString(string buildingName)
            {
                objectButton.name = field.text = string.IsNullOrEmpty(buildingName) ? building.name : buildingName;
            }

            objectButton.Add(field);

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
            
            bool hasBuilding = TMBuildingDataManager.Instance.BuildingDataList.Contains(building);
            
            Button addButton = new()
            {
                style =
                {
                    position = Position.Absolute,
                    right = new Length(10, LengthUnit.Percent),
                    top = new Length(50, LengthUnit.Percent),                    
                    translate = new Translate(0, Length.Percent(-50)),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = ColorUtility.TryParseHtmlString(
                        hasBuilding ? "#4447A6" : "#2B2D6D", 
                        out Color addButtonColor) ? 
                        addButtonColor : 
                        Color.blue,
                    height = 20,
                    width = 20,
                },
                text = hasBuilding ? "On" : "Off",
            };

            addButton.clicked += () =>
            {
                bool hasCardByClicked = TMBuildingDataManager.Instance.BuildingDataList.Contains(building);

                if (hasCardByClicked)
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#2B2D6D", out Color color) ? color : Color.blue;
                    addButton.text = "Off";
                    TMBuildingDataManager.Instance.RemoveBuilding(building);
                }
                else
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#4447A6", out Color color) ? color : Color.blue;
                    addButton.text = "On";
                    TMBuildingDataManager.Instance.AddBuilding(building);
                }
                
                addButton.SetChangedColorButtonEvent();
            };

            addButton.SetChangedColorButtonEvent();
            removeButton.SetChangedColorButtonEvent();
            objectButton.Add(addButton);
            objectButton.Add(removeButton);
            return objectButton;
        }

        internal override void OnDisable()
        {
            _buildingNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif
