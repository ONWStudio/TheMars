using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using Onw.Extensions;

namespace Onw.Localization
{
    [System.Serializable]
    public sealed class LocalizedStringOption
    {
        public string EntryKeyName 
            => !string.IsNullOrEmpty(_localizedString.TableReference) ? 
                LocalizationSettings
                    .StringDatabase
                    .GetTable(_localizedString.TableReference)?
                    .GetEntry(_localizedString.TableEntryReference.KeyId)?
                    .Key ?? "" : 
                "";

        [SerializeField]
        private LocalizedString _localizedString;

        public bool TrySetOption(MonoBehaviour monoBehaviour, UnityAction<string> onChangedText, out LocalizeStringEvent localizeStringEvent)
        {
            if (!monoBehaviour)
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

            localizeStringEvent.OnUpdateString.AddListener(onChangedText);
            localizeStringEvent.OnUpdateString.SetPersistentListenerState(UnityEventCallState.EditorAndRuntime);
            localizeStringEvent.RefreshString();

            return true;

            static LocalizeStringEvent getLocalizeStringEvent(GameObject go, LocalizedString localizedString)
            {
                LocalizeStringEvent localizeStringEvent = go.AddComponent<LocalizeStringEvent>();
                localizeStringEvent.StringReference = localizedString;

                return localizeStringEvent;
            }
        }

        public LocalizedStringOption(TableReference tableReference, TableEntryReference entryReference)
        {
            _localizedString = new(tableReference, entryReference);
        }
    }
}
