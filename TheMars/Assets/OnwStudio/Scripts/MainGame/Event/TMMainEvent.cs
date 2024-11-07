using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Attribute;

namespace TM.Event
{
    public interface ITMEvent
    {
        event UnityAction<TMEventChoice> OnFireEvent;
        ITMEventData EventReadData { get; }
    }
    
    [System.Serializable]
    public sealed class TMEventRunner : ITMEvent
    {
        public event UnityAction<TMEventChoice> OnFireEvent
        {
            add => _onFireEvent.AddListener(value);
            remove => _onFireEvent.RemoveListener(value);
        }

        public ITMEventData EventReadData => EventData;

        [field: SerializeField] public TMEventData EventData { get; private set; }
        
        [SerializeField, ReadOnly] private UnityEvent<TMEventChoice> _onFireEvent;

        public bool CanFireTop => EventData && EventData.CanFireTop;
        public bool CanFireBottom => EventData && EventData.CanFireBottom;

        public void InvokeEvent(TMEventChoice eventChoice)
        {
            EventData.InvokeEvent(eventChoice);
            _onFireEvent.Invoke(eventChoice);
        }
        
        public TMEventRunner(TMEventData eventData)
        {
            EventData = eventData;
        }
    }
}