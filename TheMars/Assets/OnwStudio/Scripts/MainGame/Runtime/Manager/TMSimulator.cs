using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;

namespace TM.Manager
{
    public sealed class TMSimulator : MonoBehaviour
    {
        private const int INTERVAL_MIN = 5;
        private const int INTERVAL_MAX = 15;

        [field: SerializeField, ReadOnly] public float AccumulatedTime { get; private set; } = 0f;
        [field: SerializeField, ReadOnly] public int NowDay { get; private set; } = 1;

        public event UnityAction<int> OnChangedDay
        {
            add => _onChangedDay.AddListener(value);
            remove => _onChangedDay.RemoveListener(value);
        }
        
        public int IntervalInMinutes
        {
            get => _intervalInMinutes;
            set => _intervalInMinutes = Mathf.Clamp(value, INTERVAL_MIN, INTERVAL_MAX);
        }

        [SerializeField, Range(INTERVAL_MIN, INTERVAL_MAX)] private int _intervalInMinutes = 10;

        [Header("Event")]
        [SerializeField] private UnityEvent<int> _onChangedDay = new();
        
        private int _prevDay = 1;
        private float _intervalInSeconds;
        
        private void Start()
        {
            _intervalInSeconds = _intervalInMinutes * 60f;
        }

        private void Update()
        {
            AccumulatedTime += Time.deltaTime * TimeManager.GameSpeed;

            _prevDay = NowDay;
            NowDay = (int)(AccumulatedTime / _intervalInSeconds + 1);

            if (NowDay > _prevDay)
            {
                _onChangedDay.Invoke(NowDay);
            }
        }
    }
}
