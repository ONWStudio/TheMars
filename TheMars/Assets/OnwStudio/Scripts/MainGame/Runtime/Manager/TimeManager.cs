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

        public static IReadOnlyReactiveProperty<int> TimeScale => _timeScale;
 
        private static ReactiveProperty<int> _timeScale = new(1);

        public static void SetTimeScale(int scale)
        {
            _timeScale.Value = Mathf.Clamp(scale, SPEED_MIN, SPEED_MAX);
        }
        
        public static float SecondsToMinutes(float seconds) => seconds / 60f;
        public static float MinutesToSeconds(float minutes) => minutes * 60f;
    }
}