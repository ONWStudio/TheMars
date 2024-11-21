#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using Unity.EditorCoroutines.Editor;
using Onw.Extensions;
using Onw.Editor.Window;
using Onw.Editor.Extensions;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Building;
using TM.Synergy;

namespace TM.Editor
{
    internal sealed class TMSynergyScrollView : ScriptableObjectScrollView
    {
        private readonly List<EditorCoroutine> _synergyNameObservers = new();

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
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMSynergyData>("Assets/OnwStudio/ScriptableObject/Synergies", $"Synergy_No.{Guid.NewGuid()}")));
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
            TMSynergyData synergy = (objectButton.ScriptableObject as TMSynergyData)!;

            objectButton.text = "";

            Label field = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };

            if (synergy.TryGetFieldByName(
                EditorReflectionHelper.GetBackingFieldName("LocalizedSynergyName"),
                BindingFlags.NonPublic | BindingFlags.Instance,
                out LocalizedString localizedSynergyName))
            {
                _synergyNameObservers.Add(localizedSynergyName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
                if (localizedSynergyName.IsEmpty)
                {
                    objectButton.name = field.text = synergy.name;
                }
                else
                {
                    objectButton.name = field.text = localizedSynergyName.GetLocalizedString();
                }

                void onChangedString(string synergyName)
                {
                    objectButton.name = field.text = string.IsNullOrEmpty(synergyName) ? synergy.name : synergyName;
                }
            }
            else
            {
                objectButton.name = field.text = synergy.name;
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
            
            bool hasSynergy = TMSynergyDataManager.Instance.SynergyDataList.Contains(synergy);
            
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
                        hasSynergy ? "#4447A6" : "#2B2D6D", 
                        out Color addButtonColor) ? 
                        addButtonColor : 
                        Color.blue,
                    height = 20,
                    width = 20,
                },
                text = hasSynergy ? "On" : "Off",
            };

            addButton.clicked += () =>
            {
                bool hasCardByClicked = TMSynergyDataManager.Instance.SynergyDataList.Contains(synergy);

                if (hasCardByClicked)
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#2B2D6D", out Color color) ? color : Color.blue;
                    addButton.text = "Off";
                    TMSynergyDataManager.Instance.RemoveSynergyData(synergy);
                }
                else
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#4447A6", out Color color) ? color : Color.blue;
                    addButton.text = "On";
                    TMSynergyDataManager.Instance.AddSynergyData(synergy);
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
            _synergyNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif