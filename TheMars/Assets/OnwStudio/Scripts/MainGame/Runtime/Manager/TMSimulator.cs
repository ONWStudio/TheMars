using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Manager;
using Onw.Attribute;

namespace TM.Manager
{
    public sealed class TMSimulator : SceneSingleton<TMSimulator>
    {
        public override string SceneName => "MainGameScene";
        
        private const int INTERVAL_MIN = 5;
        private const int INTERVAL_MAX = 15;

        [field: SerializeField, ReadOnly] public float AccumulatedTime { get; private set; } = 0f;
        [field: SerializeField, ReadOnly] public int NowDay { get; private set; } = 1;
        [field: SerializeField, ReadOnly] public int NowMinutes { get; private set; } = 0;
        [field: SerializeField, ReadOnly] public int NowSeconds { get; private set; } = 0;

        public event UnityAction<int> OnChangedDay
        {
            add => _onChangedDay.AddListener(value);
            remove => _onChangedDay.RemoveListener(value);
        }
        
        public event UnityAction<int> OnChangedMinutes
        {
            add => _onChangedMinutes.AddListener(value);
            remove => _onChangedMinutes.RemoveListener(value);
        }
        
        public event UnityAction<int> OnChangedSeconds
        {
            add => _onChangedSeconds.AddListener(value);
            remove => _onChangedSeconds.RemoveListener(value);
        }
        
        public int IntervalInMinutes
        {
            get => _intervalInMinutes;
            set => _intervalInMinutes = Mathf.Clamp(value, INTERVAL_MIN, INTERVAL_MAX);
        }

        [SerializeField, Range(INTERVAL_MIN, INTERVAL_MAX)] private int _intervalInMinutes = 10;

        [Header("Event")]
        [SerializeField] private UnityEvent<int> _onChangedDay = new();
        [SerializeField] private UnityEvent<int> _onChangedMinutes = new();
        [SerializeField] private UnityEvent<int> _onChangedSeconds = new();
        
        private int _prevDay = 1;
        private int _prevMinutes = 0;
        private int _prevSeconds = 0;
        private float _intervalInSeconds;

        protected override void Init() {}

        private void Start()
        {
            _intervalInSeconds = _intervalInMinutes * 60f;
        }

        private void Update()
        {
            AccumulatedTime += Time.deltaTime * TimeManager.GameSpeed;

            _prevSeconds = NowSeconds;
            _prevMinutes = NowMinutes;
            _prevDay = NowDay;

            NowSeconds = (int)(AccumulatedTime / 1);
            NowMinutes = (int)(AccumulatedTime / 60);
            NowDay = (int)(AccumulatedTime / _intervalInSeconds + 1);

            if (NowSeconds > _prevSeconds)
            {
                _onChangedSeconds.Invoke(NowSeconds);
            }

            if (NowMinutes > _prevMinutes)
            {
                _onChangedMinutes.Invoke(NowMinutes);
            }
            
            if (NowDay > _prevDay)
            {
                _onChangedDay.Invoke(NowDay);
            }
        }
    }
}
