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
        [SerializeField, ReadOnly] private ReactiveField<int> _nowDay = new() { Value = 1 };
        [SerializeField, ReadOnly] private ReactiveField<int> _nowMinutes = new() { Value = 0 };
        [SerializeField, ReadOnly] private ReactiveField<int> _nowSeconds = new() { Value = 0 };

        public IReactiveField<int> NowDay => _nowDay;
        public IReactiveField<int> NowMinutes => _nowMinutes;
        public IReactiveField<int> NowSeconds => _nowSeconds;
        
        public int IntervalInMinutes
        {
            get => _intervalInMinutes;
            set => _intervalInMinutes = Mathf.Clamp(value, INTERVAL_MIN, INTERVAL_MAX);
        }

        [field: SerializeField, ReadOnly] public float IntervalInSeconds { get; private set; } = 0f;

        [SerializeField, Range(INTERVAL_MIN, INTERVAL_MAX)] private int _intervalInMinutes = 2;

        protected override void Init() {}

        private void Start()
        {
            IntervalInSeconds = _intervalInMinutes * 60f;
        }

        private void Update()
        {
            AccumulatedTime += Time.deltaTime * TimeManager.GameSpeed;

            _nowSeconds.Value = (int)(AccumulatedTime / 1);
            _nowMinutes.Value = (int)(AccumulatedTime / 60);
            _nowDay.Value = (int)(AccumulatedTime / IntervalInSeconds + 1);
        }
    }
}
