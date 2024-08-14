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
                long keyId = _localizedString.TableEntryReference.KeyId;

                if (!string.IsNullOrEmpty(_localizedString.TableReference))
                {
                    var stringTable = LocalizationSettings.StringDatabase.GetTable(_localizedString.TableReference);

                    if (stringTable)
                    {
                        var entry = stringTable.GetEntry(keyId);
                        if (entry is not null)
                        {
                            return entry.Key;
                        }
                    }
                }

                return "";
            }
        }
        

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
