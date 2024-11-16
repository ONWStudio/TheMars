#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Unity.EditorCoroutines.Editor;
using Onw.Helper;
using Onw.Editor.Window;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Event;
using TM.Synergy;
using UnityEngine.UIElements;
using Onw.Editor.Extensions;
using TM.Card;
using System.Reflection;

namespace TM.Editor
{
    internal sealed class TMEventScrollView : ScriptableObjectScrollView
    {
        private readonly List<EditorCoroutine> _eventNameObservers = new();

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

            Button creationButton = createHeaderButton(()
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMEventData>("Assets/OnwStudio/ScriptableObject/Events", $"Event_No.{Guid.NewGuid()}")));
            creationButton.text = "추가";
            creationButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#1B5914", out Color addColor) ? addColor : Color.green;
            creationButton.SetChangedColorButtonEvent();
            header.Add(creationButton);

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
            TMEventData eventData = (so as TMEventData)!;
            LocalizedString localizedEventName = eventData.TitleTextEvent;

            objectButton.text = "";

            Label label = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };

            objectButton.Add(label);

            Button removeButton = new(() =>
            {
                if (_selectedObject?.ScriptableObject == so)
                {
                    _selectedObject = null;
                }

                RemoveSo(so);
                TMEventDataManager.Instance.RemoveEventFromPositive(eventData);
                TMEventDataManager.Instance.RemoveEventFromNegative(eventData);
                TMEventDataManager.Instance.RemoveEventFromCalamity(eventData);
                
                if (TMEventDataManager.Instance.ExpansionEventData == eventData)
                {
                    TMEventDataManager.Instance.ExpansionEventData = null;
                }

                if (TMEventDataManager.Instance.MarsLithiumEvent == eventData)
                {
                    TMEventDataManager.Instance.MarsLithiumEvent = null;
                }

                if (TMEventDataManager.Instance.RootMainEventData == eventData)
                {
                    TMEventDataManager.Instance.RootMainEventData = null;
                }


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

            DropdownField dropdownField = new()
            {
                style =
                {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = new Length(2, LengthUnit.Percent),
                    top = new Length(50, LengthUnit.Percent),
                    translate = new Translate(0, Length.Percent(-50)),
                    height = 20
                },
                choices = new()
                {
                    "메인 이벤트 루트",
                    "마르스 리튬 납부",
                    "확장 이벤트",
                    "긍정",
                    "부정",
                    "재난",
                    "NONE"
                },
                index = eventData switch
                {
                    _ when eventData == TMEventDataManager.Instance.RootMainEventData => 0,
                    _ when eventData == TMEventDataManager.Instance.MarsLithiumEvent => 1,
                    _ when eventData == TMEventDataManager.Instance.ExpansionEventData => 2,
                    _ when TMEventDataManager.Instance.PositiveEventList.Contains(eventData) => 3,
                    _ when TMEventDataManager.Instance.NegativeEventList.Contains(eventData) => 4,
                    _ when TMEventDataManager.Instance.CalamityEventList.Contains(eventData) => 5,
                    _ => 6
                }
            };

            dropdownField.RegisterValueChangedCallback(evt =>
            {
                switch (evt.newValue)
                {
                    case "메인 이벤트 루트":
                        if (TMEventDataManager.Instance.RootMainEventData != eventData)
                        {
                            ScriptableObjectButton prevRoot = View
                                .Children()
                                .OfType<ScriptableObjectButton>()
                                .Where(t => t != objectButton)
                                .SingleOrDefault(t => TMEventDataManager.Instance.RootMainEventData == t.ScriptableObject);
                            
                            DropdownField prevRootDropdownField = prevRoot?.Q<DropdownField>();
                            if (prevRootDropdownField is not null)
                            {
                                prevRootDropdownField.index = 6;
                            }

                            TMEventDataManager.Instance.RootMainEventData = eventData;
                        }
                        break;
                    case "마르스 리튬 납부":
                        if (TMEventDataManager.Instance.MarsLithiumEvent != eventData)
                        {
                            ScriptableObjectButton prevRoot = View
                                .Children()
                                .OfType<ScriptableObjectButton>()
                                .Where(t => t != objectButton)
                                .SingleOrDefault(t => TMEventDataManager.Instance.MarsLithiumEvent == t.ScriptableObject);

                            DropdownField prevRootDropdownField = prevRoot?.Q<DropdownField>();
                            if (prevRootDropdownField is not null)
                            {
                                prevRootDropdownField.index = 6;
                            }

                            TMEventDataManager.Instance.MarsLithiumEvent = eventData;
                        }
                        break;
                    case "확장 이벤트":
                        if (TMEventDataManager.Instance.ExpansionEventData != eventData)
                        {
                            ScriptableObjectButton prevRoot = View
                                .Children()
                                .OfType<ScriptableObjectButton>()
                                .Where(t => t != objectButton)
                                .SingleOrDefault(t => TMEventDataManager.Instance.ExpansionEventData == t.ScriptableObject);

                            DropdownField prevRootDropdownField = prevRoot?.Q<DropdownField>();
                            if (prevRootDropdownField is not null)
                            {
                                prevRootDropdownField.index = 6;
                            }

                            TMEventDataManager.Instance.ExpansionEventData = eventData;
                        }
                        break;
                    case "긍정":
                        TMEventDataManager.Instance.AddEventFromPositive(eventData);
                        break;
                    case "부정":
                        TMEventDataManager.Instance.AddEventFromNegative(eventData);
                        break;
                    case "재난":
                        TMEventDataManager.Instance.AddEventFromCalamity(eventData);
                        break;
                    default:
                        if (TMEventDataManager.Instance.RootMainEventData == eventData)
                        {
                            TMEventDataManager.Instance.RootMainEventData = null;
                        }
                        
                        if (TMEventDataManager.Instance.ExpansionEventData == eventData)
                        {
                            TMEventDataManager.Instance.ExpansionEventData = null;
                        }

                        if (TMEventDataManager.Instance.MarsLithiumEvent == eventData)
                        {
                            TMEventDataManager.Instance.MarsLithiumEvent = null;
                        }

                        TMEventDataManager.Instance.RemoveEventFromPositive(eventData);
                        TMEventDataManager.Instance.RemoveEventFromNegative(eventData);
                        TMEventDataManager.Instance.RemoveEventFromCalamity(eventData);
                        break;
                }
            });
            
            _eventNameObservers.Add(localizedEventName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
            if (localizedEventName.IsEmpty)
            {
                objectButton.name = label.text = eventData.name;
            }
            else
            {
                objectButton.name = label.text = localizedEventName.GetLocalizedString();
            }

            objectButton.Add(removeButton);
            objectButton.Add(dropdownField);

            return objectButton;

            void onChangedString(string eventName)
            {
                objectButton.name = label.text = string.IsNullOrEmpty(eventName) ? eventData.name : eventName;
            }
        }

        internal override void OnDisable()
        {
            _eventNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif