#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Editor.Extensions;
using Onw.Editor.Window;
using Onw.Extensions;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Building;
using TM.Synergy;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace TM.Editor
{
    internal sealed class TMSynergyScrollView : ScriptableObjectScrollView
    {
        private EditorCoroutine _synergyNameObserver;

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
            ScriptableObjectButton button = base.CreateButton(so);
            TMSynergyData synergy = (button.ScriptableObject as TMSynergyData)!;

            button.text = "";

            Label field = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };

            if (synergy.TryGetFieldByName(
                "_localizedSynergyName",
                BindingFlags.NonPublic | BindingFlags.Instance,
                out LocalizedString localizedSynergyName))
            {
                _synergyNameObserver = localizedSynergyName.MonitorSpecificLocaleEntry("ko-KR", onChangedString);
                if (localizedSynergyName.IsEmpty)
                {
                    button.name = field.text = synergy.name;
                }
                else
                {
                    button.name = field.text = localizedSynergyName.GetLocalizedString();
                }

                void onChangedString(string synergyName)
                {
                    button.name = field.text = string.IsNullOrEmpty(synergyName) ? synergy.name : synergyName;
                }
            }
            else
            {
                button.name = field.text = synergy.name;
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
            if (_synergyNameObserver is not null)
            {
                EditorCoroutineUtility.StopCoroutine(_synergyNameObserver);
            }
        }
    }
}
#endif