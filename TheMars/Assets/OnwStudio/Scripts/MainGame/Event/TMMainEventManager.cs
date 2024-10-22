using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Manager;
using TM.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace TM.Event
{
    public sealed class TMMainEventManager : SceneSingleton<TMMainEventManager>
    {
        protected override string SceneName => "MainGameScene";

        public ITMEvent CurrentEvent => _currentEventData;
        
        public event UnityAction<TMEventData> OnTriggerMainEvent
        {
            add => _onTriggerMainEvent.AddListener(value);
            remove => _onTriggerMainEvent.RemoveListener(value);
        }
        
        [field: SerializeField] public int CheckDayCount { get; private set; } = 5;
        
        [SerializeField] private TMEventData _currentEventData = null;
        
        [Header("Event")]
        [SerializeField] private UnityEvent<TMEventData> _onTriggerMainEvent = new();
        
        private bool _isApplicationQuit = false;
        
        protected override void Init()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
        }

        private void onChangedDay(int day)
        {
            if (day % CheckDayCount != 0) return;
            
            _onTriggerMainEvent.Invoke(_currentEventData);
            
            TMEventData eventData = _currentEventData;
            eventData.OnFireEvent += onFireEvent;

            void onFireEvent(TMEventChoice eventChoice)
            {
                if (!eventData) return;
                
                eventData.OnFireEvent -= onFireEvent;
                _currentEventData = eventChoice == TMEventChoice.LEFT ? eventData.LeftEvent : eventData.RightEvent;
            }
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        private void OnDestroy()
        {
            if (_isApplicationQuit) return;

            TMSimulator.Instance.OnChangedDay -= onChangedDay;
        }
    }
}
