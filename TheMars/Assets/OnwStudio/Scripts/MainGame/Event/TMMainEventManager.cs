using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Manager;
using TM.Manager;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMMainEventManager : SceneSingleton<TMMainEventManager>
    {
        protected override string SceneName => "MainGameScene";

        public ITMEvent CurrentEvent => _currentEventData;
        
        [field: SerializeField] public int CheckDayCount { get; private set; } = 5;
        
        [SerializeField]
        private TMEventData _currentEventData = null;

        private bool _isApplicationQuit = false;
        
        protected override void Init()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
        }

        private void onChangedDay(int day)
        {
            if (day % CheckDayCount != 0) return;
            
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        private void OnDestroy()
        {
            if (_isApplicationQuit) return;

            TMSimulator.Instance.OnChangedDay -= onChangedDay();
        }
    }
}
