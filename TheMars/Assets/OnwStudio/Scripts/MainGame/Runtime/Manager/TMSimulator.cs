using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Manager;
using Onw.Attribute;
using Onw.Event;
using UniRx;

namespace TM.Manager
{
    public sealed class TMSimulator : SceneSingleton<TMSimulator>
    {
        protected override string SceneName => "MainGameScene";
        
        private const int INTERVAL_MIN = 1;
        private const int INTERVAL_MAX = 15;

        [field: SerializeField, ReadOnly] public float AccumulatedTime { get; private set; } = 0f;
        [SerializeField] private ReactiveField<int> _nowDay = new() { Value = 1 };
        [SerializeField] private ReactiveField<int> _nowMinutes = new() { Value = 0 };
        [SerializeField] private ReactiveField<int> _nowSeconds = new() { Value = 0 };

        public IReactiveField<int> NowDay => _nowDay;
        public IReactiveField<int> NowMinutes => _nowMinutes;
        public IReactiveField<int> NowSeconds => _nowSeconds;
        
        public int IntervalInMinutes
        {
            get => _intervalInMinutes;
            set => _intervalInMinutes = Mathf.Clamp(value, INTERVAL_MIN, INTERVAL_MAX);
        }

        public int IntervalInSeconds 
        { 
            get
            {
                _intervalInSeconds ??= _intervalInMinutes * 60;
                return (int)_intervalInSeconds;
            }
        }

        [SerializeField, Range(INTERVAL_MIN, INTERVAL_MAX)] private int _intervalInMinutes = 2;

        private int? _intervalInSeconds = null;

        protected override void Init() {}

        private void Update()
        {
            AccumulatedTime += Time.deltaTime * TimeManager.TimeScale;

            _nowSeconds.Value = (int)(AccumulatedTime / 1);
            _nowMinutes.Value = (int)(AccumulatedTime / 60);
            _nowDay.Value = (int)(AccumulatedTime / IntervalInSeconds + 1);
        }
    }
}
