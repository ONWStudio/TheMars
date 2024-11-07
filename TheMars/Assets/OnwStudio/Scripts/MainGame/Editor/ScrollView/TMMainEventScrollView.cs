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

namespace TM.Editor
{
    internal sealed class TMMainEventScrollView : ScriptableObjectScrollView
    {
        private readonly List<EditorCoroutine> _eventNameObservers = new();

        internal override void OnInitialize()
        {
            Type[] types = ReflectionHelper.GetChildTypesFromBaseType<TMEventData>().ToArray();

            TMEventData[] unmadeTypes = types
                .Where(t => _scriptableObjects.All(button => button.ScriptableObject.GetType() != t))
                .Select(t => ScriptableObjectHandler.CreateScriptableObject("Assets/OnwStudio/ScriptableObject/Events", $"Event_No.{Guid.NewGuid().ToString()}", t))
                .OfType<TMEventData>()
                .ToArray();

            AddRange(unmadeTypes);
        }

        protected override ScriptableObjectButton CreateButton(ScriptableObject so)
        {
            ScriptableObjectButton objectButton = base.CreateButton(so);
            TMEventData eventData = (so as TMEventData)!;
            LocalizedString localizedEventName = eventData.TitleTextEvent;

            DropdownField dropdownField = new()
            {
                style =
                {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    right = new Length(2, LengthUnit.Percent),
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
                objectButton.name = objectButton.text = eventData.name;
            }
            else
            {
                objectButton.name = objectButton.text = localizedEventName.GetLocalizedString();
            }
            
            objectButton.Add(dropdownField);

            return objectButton;

            void onChangedString(string eventName)
            {
                objectButton.name = objectButton.text = string.IsNullOrEmpty(eventName) ? eventData.name : eventName;
            }
        }

        internal override void OnDisable()
        {
            _eventNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif