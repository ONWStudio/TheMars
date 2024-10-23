using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Manager;
using Onw.Attribute;
using TM.Manager;

namespace TM.Event
{
    public sealed class TMMainEventManager : SceneSingleton<TMMainEventManager>
    {
        protected override string SceneName => "MainGameScene";

        public event UnityAction<TMMainEvent> OnTriggerMainEvent
        {
            add => _onTriggerMainEvent.AddListener(value);
            remove => _onTriggerMainEvent.RemoveListener(value);
        }
        
        [field: SerializeField] public int CheckDayCount { get; private set; } = 5;
        
        [SerializeField] private TMMainEvent _mainEvent = null;
        
        [Header("Event")]
        [SerializeField] private UnityEvent<TMMainEvent> _onTriggerMainEvent = new();
        
        private bool _isApplicationQuit = false;
        
        protected override void Init()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
        }

        private void Start()
        {
            onChangedDay(0);
        }

        private void onChangedDay(int day)
        {
            if (day % CheckDayCount != 0) return;
            
            _onTriggerMainEvent.Invoke(_mainEvent);

            TimeManager.IsFreeze = true;
            TMMainEvent mainEvent = _mainEvent;
            mainEvent.OnFireEvent += onFireEvent;

            void onFireEvent(TMEventChoice eventChoice)
            {
                if (mainEvent is null) return;
                
                TimeManager.IsFreeze = false;
                mainEvent.OnFireEvent -= onFireEvent;
                _mainEvent = new(eventChoice == TMEventChoice.TOP ? 
                    mainEvent.EventData.TopEventData : 
                    mainEvent.EventData.BottomEventData);
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
