using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Manager
{
    public static class TimeManager
    {
        private const int SPEED_MAX = 3;
        private const int SPEED_MIN = 1;

        public static int GameSpeed
        {
            get => _gameSpeed;
            set => _gameSpeed = Mathf.Clamp(value, SPEED_MIN, SPEED_MAX);
        }

        private static int _gameSpeed = 1;

        public static float SecondsToMinutes(float seconds) => seconds / 60f;
        public static float MinutesToSeconds(float minutes) => minutes * 60f;
    }
}

