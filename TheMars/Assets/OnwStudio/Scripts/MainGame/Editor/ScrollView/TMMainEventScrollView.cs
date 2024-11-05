#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Unity.EditorCoroutines.Editor;
using Onw.Helper;
using Onw.Extensions;
using Onw.Editor.Window;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Event;

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
            ScriptableObjectButton button = base.CreateButton(so);
            TMEventData eventData = (so as TMEventData)!;
            LocalizedString localizedEventName = eventData.TitleTextEvent;

            _eventNameObservers.Add(localizedEventName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
            if (localizedEventName.IsEmpty)
            {
                button.name = button.text = eventData.name;
            }
            else
            {
                button.name = button.text = localizedEventName.GetLocalizedString();
            }

            return button;

            void onChangedString(string eventName)
            {
                button.name = button.text = string.IsNullOrEmpty(eventName) ? eventData.name : eventName;
            }
        }

        internal override void OnDisable()
        {
            _eventNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif