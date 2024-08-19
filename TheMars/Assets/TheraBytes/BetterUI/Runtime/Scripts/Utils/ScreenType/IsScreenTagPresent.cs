using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheraBytes.BetterUi
{
    [Serializable]
    public class IsScreenTagPresent : IScreenTypeCheck
    {
        [SerializeField]
        private string screenTag;
        public string ScreenTag { get { return screenTag; } set { screenTag = value; } }

        [SerializeField]
        private bool isActive;

        public bool IsActive { get { return isActive; } set { isActive = value; } }

        public bool IsScreenType()
        {
            HashSet<string> curentTags = ResolutionMonitor.CurrentScreenTags as HashSet<string>;
            return curentTags.Contains(screenTag);
        }
    }
}
