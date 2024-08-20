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
        {
            get
            {
                if (!string.IsNullOrEmpty(_localizedString.TableReference))
                {
                    LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult t = LocalizationSettings.StringDatabase.GetTableEntry(_localizedString.TableReference, _localizedString.TableEntryReference);
                    return t.Entry?.Key ?? "";
                }

                return "";
            }
        }

        [SerializeField]
        private LocalizedString _localizedString;

        public bool TrySetOption(MonoBehaviour monoBehaviour, UnityAction<string> onChangedText, out LocalizeStringEvent localizeStringEvent)
        {
            if (string.IsNullOrEmpty(EntryKeyName) || !monoBehaviour) // .. 모노비하이비어가 없다면..
            {
                localizeStringEvent = null;
                return false;
            }

            if (!monoBehaviour.TryGetComponent(out localizeStringEvent)) // .. 
            {
                localizeStringEvent = getLocalizeStringEvent(monoBehaviour.gameObject, _localizedString);
            }
            else
            {
                if (localizeStringEvent.StringReference != _localizedString)
                {
                    localizeStringEvent = getLocalizeStringEvent(monoBehaviour.gameObject, _localizedString);
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