using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheraBytes.BetterUi
{
    [Serializable]
    public class IsCertainScreenOrientation : IScreenTypeCheck
    {
        public enum Orientation
        {
            Portrait,
            Landscape,
        }

        [SerializeField]
        private Orientation expectedOrientation;
        public Orientation ExpectedOrientation { get { return expectedOrientation; } set { expectedOrientation = value; } }

        [SerializeField]
        private bool isActive;
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        public IsCertainScreenOrientation(Orientation expectedOrientation)
        {
            this.expectedOrientation = expectedOrientation;
        }

        public bool IsScreenType()
        {
            Vector2 res = ResolutionMonitor.CurrentResolution;

            switch (expectedOrientation)
            {
                case Orientation.Portrait:
                    return res.x < res.y;

                case Orientation.Landscape:
                    return res.x >= res.y;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
