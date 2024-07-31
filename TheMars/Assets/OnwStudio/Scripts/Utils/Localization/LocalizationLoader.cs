using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Onw.Extensions;
using Onw.Attribute;
using TMPro;

namespace Onw.Localization
{
    [Serializable]
    public sealed class LocalizedStringOption
    {
        [SerializeField, ReadOnly] private LocalizedString _localizedString;

        public bool TrySetOption(MonoBehaviour monoBehaviour, TextMeshProUGUI tmpText, out LocalizeStringEvent localizeStringEvent)
        {
            if (!tmpText || !monoBehaviour)
            {
                localizeStringEvent = null;
                return false;
            }

            if (!monoBehaviour.TryGetComponent(out localizeStringEvent))
            {
                localizeStringEvent = getLocalizeStringEvent(monoBehaviour.gameObject, _localizedString);
            }
            else
            {
                if (localizeStringEvent.StringReference is null || !ReferenceEquals(localizeStringEvent.StringReference, localizeStringEvent))
                {
                    localizeStringEvent = getLocalizeStringEvent(monoBehaviour.gameObject, _localizedString);
                }
                else
                {
                    localizeStringEvent.OnUpdateString.RemoveAllListeners();
                    localizeStringEvent.StringReference = _localizedString;
                }
            }

            localizeStringEvent.OnUpdateString.AddListener(text => tmpText.text = text);
            localizeStringEvent.OnUpdateString.SetPersistentListenerState(UnityEngine.Events.UnityEventCallState.EditorAndRuntime);
            localizeStringEvent.RefreshString();

            static LocalizeStringEvent getLocalizeStringEvent(GameObject go, LocalizedString localizedString)
            {
                LocalizeStringEvent localizeStringEvent = go.AddComponent<LocalizeStringEvent>();
                localizeStringEvent.StringReference = localizedString;

                return localizeStringEvent;
            }

            return true;
        }

        public LocalizedStringOption(TableReference tableReference, TableEntryReference entryReference)
        {
            _localizedString = new(tableReference, entryReference);
        }
    }
}
