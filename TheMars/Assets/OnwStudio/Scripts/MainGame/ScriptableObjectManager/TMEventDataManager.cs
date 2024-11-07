using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;
using Onw.Attribute;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TM.Event
{
    public sealed class TMEventDataManager : ScriptableObjectSingleton<TMEventDataManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/ScriptableObjectManager/Resources/TMEventDataManager";

        public IReadOnlyList<TMEventData> PositiveEventList => _positiveEventList;
        public IReadOnlyList<TMEventData> NegativeEventList => _negativeEventList;
        public IReadOnlyList<TMEventData> CalamityEventList => _calamityEventList;

        public TMEventData MarsLithiumEvent
        {
            get => _marsLithiumEventData;
            #if UNITY_EDITOR
            internal set
            {
                _marsLithiumEventData = value;

                if (_marsLithiumEventData is not null)
                {
                    removeEventFromEventList(_marsLithiumEventData);

                    if (_marsLithiumEventData == _expansionEventData)
                    {
                        _expansionEventData = null;
                    }

                    if (_marsLithiumEventData == _rootMainEventData)
                    {
                        _rootMainEventData = null;
                    }
                }

                EditorUtility.SetDirty(this);
            }
            #endif
        }

        public TMEventData ExpansionEventData
        {
            get => _expansionEventData;
            #if UNITY_EDITOR
            internal set
            {
                _expansionEventData = value;

                if (_expansionEventData is not null)
                {
                    removeEventFromEventList(_expansionEventData);

                    if (_expansionEventData == _marsLithiumEventData)
                    {
                        _marsLithiumEventData = null;
                    }

                    if (_expansionEventData == _rootMainEventData)
                    {
                        _rootMainEventData = null;
                    }
                }

                EditorUtility.SetDirty(this);
            }
            #endif
        }

        public TMEventData RootMainEventData
        {
            get => _rootMainEventData;
            #if UNITY_EDITOR
            internal set
            {
                _rootMainEventData = value;

                if (_rootMainEventData is not null)
                {
                    removeEventFromEventList(_rootMainEventData);

                    if (_rootMainEventData == _expansionEventData)
                    {
                        _expansionEventData = null;
                    }

                    if (_rootMainEventData == _marsLithiumEventData)
                    {
                        _marsLithiumEventData = null;
                    }
                }

                EditorUtility.SetDirty(this);
            }
            #endif
        }
        
        [SerializeField, ReadOnly] private TMEventData _marsLithiumEventData = null;
        [SerializeField, ReadOnly] private TMEventData _expansionEventData = null;
        [SerializeField, ReadOnly] private TMEventData _rootMainEventData = null;
        
        [SerializeField, ReadOnly] private List<TMEventData> _positiveEventList = new();
        [SerializeField, ReadOnly] private List<TMEventData> _negativeEventList = new();
        [SerializeField, ReadOnly] private List<TMEventData> _calamityEventList = new();
        
        #if UNITY_EDITOR
        internal void AddEventFromPositive(TMEventData eventData)
        {
            if (_positiveEventList.Contains(eventData)) return;
            
            setEventList(eventData);

            _negativeEventList.Remove(eventData);
            _calamityEventList.Remove(eventData);
            _positiveEventList.Add(eventData);
            EditorUtility.SetDirty(this);
        }

        internal void AddEventFromNegative(TMEventData eventData)
        {
            if (_negativeEventList.Contains(eventData)) return;
            
            setEventList(eventData);

            _positiveEventList.Remove(eventData);
            _calamityEventList.Remove(eventData);            
            _negativeEventList.Add(eventData);
            EditorUtility.SetDirty(this);
        }

        internal void AddEventFromCalamity(TMEventData eventData)
        {
            if (_calamityEventList.Contains(eventData)) return;

            setEventList(eventData);

            _positiveEventList.Remove(eventData);
            _negativeEventList.Remove(eventData);
            _calamityEventList.Add(eventData);
            EditorUtility.SetDirty(this);            
        }

        internal bool RemoveEventFromPositive(TMEventData eventData)
        {
            return removeEvent(_positiveEventList, eventData);
        }

        internal bool RemoveEventFromNegative(TMEventData eventData)
        {
            return removeEvent(_negativeEventList, eventData);
        }

        internal bool RemoveEventFromCalamity(TMEventData eventData)
        {
            return removeEvent(_calamityEventList, eventData);
        }

        private bool removeEvent(List<TMEventData> eventList, TMEventData eventData)
        {
            bool isRemove = eventList.Remove(eventData);
            EditorUtility.SetDirty(this);
            
            return isRemove;
        }
        
        private void removeEventFromEventList(TMEventData eventData)
        {
            _positiveEventList.Remove(eventData);
            _negativeEventList.Remove(eventData);
            _calamityEventList.Remove(eventData);
        }

        private void setEventList(TMEventData eventData)
        {
            if (_marsLithiumEventData == eventData)
            {
                _marsLithiumEventData = null;
            }

            if (_expansionEventData == eventData)
            {
                _expansionEventData = null;
            }

            if (_rootMainEventData == eventData)
            {
                _rootMainEventData = null;
            }
        }
        #endif

        public TMEventData[] GetEventDataArrayByWhere(Func<TMEventData, bool> predicate)
        {
            return _positiveEventList
                .Where(predicate)
                .ToArray();
        }
    }
}
