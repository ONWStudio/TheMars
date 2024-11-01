#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Unity.EditorCoroutines.Editor;
using Onw.Editor.Window;
using Onw.Localization.Editor;
using TM.Event;

namespace TM.Editor
{
    internal sealed class TMMainEventScrollView : ScriptableObjectScrollView
    {
        private EditorCoroutine _eventNameObserver = null;

        protected override ScriptableObjectButton CreateButton(ScriptableObject so)
        {
            ScriptableObjectButton button = base.CreateButton(so);
            TMEventData eventData = (so as TMEventData)!;
            LocalizedString localizedEventName = eventData.TitleTextEvent;

            _eventNameObserver = localizedEventName.MonitorSpecificLocaleEntry("ko-KR", onChangedString);
            if (localizedEventName.IsEmpty)
            {
                button.name = button.text = eventData.name;
            }
            else
            {
                button.name = button.text = localizedEventName.GetLocalizedString();
            }

            return button;

            void onChangedString(string synergyName)
            {
                button.name = button.text = string.IsNullOrEmpty(synergyName) ? eventData.name : synergyName;
            }
        }

        internal override void OnDisable()
        {
            if (_eventNameObserver is not null)
            {
                EditorCoroutineUtility.StopCoroutine(_eventNameObserver);
            }
        }
    }
}
#endif