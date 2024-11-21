using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TM.Manager
{
    public static class TimeManager
    {
        private const int SPEED_MAX = 5;
        private const int SPEED_MIN = 0;

        public static bool IsPause { get; set; } = false;

        public static int TimeScale => IsPause ? 0 : _timeScale.Value;
 
        private static readonly ReactiveProperty<int> _timeScale = new(1);

        private static readonly Dictionary<Action<int>, IDisposable> _subscriptions = new();

        public static void AddListenerOnChangedTimeScale(Action<int> observer)
        {
            _subscriptions[observer] = _timeScale.Subscribe(observer);
        }

        public static void RemoveListenerOnChangedTimeScale(Action<int> observer)
        {
            if (!_subscriptions.TryGetValue(observer, out IDisposable subscription)) return;
            
            subscription.Dispose();
            _subscriptions.Remove(observer);
        }

        public static void SetTimeScale(int scale)
        {
            _timeScale.Value = Mathf.Clamp(scale, SPEED_MIN, SPEED_MAX);
        }
    }
}