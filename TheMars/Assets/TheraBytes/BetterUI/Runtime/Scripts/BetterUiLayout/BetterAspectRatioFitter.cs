﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [HelpURL("https://documentation.therabytes.de/better-ui/BetterAspectRatioFitter.html")]
    [AddComponentMenu("Better UI/Layout/Better Aspect Ratio Fitter", 30)]
    public class BetterAspectRatioFitter : AspectRatioFitter, IResolutionDependency
    {
        [Serializable]
        public class Settings : IScreenConfigConnection
        {
            public AspectMode AspectMode;
            public float AspectRatio = 1;

            [SerializeField]
            private string screenConfigName;
            public string ScreenConfigName { get { return screenConfigName; } set { screenConfigName = value; } }
        }

        [Serializable]
        public class SettingsConfigCollection : SizeConfigCollection<Settings> { }

        public Settings CurrentSettings { get { return customSettings.GetCurrentItem(settingsFallback); } }

        public new AspectMode aspectMode
        {
            get { return base.aspectMode; }
            set
            {
                Config.Set(value, (o) => base.aspectMode = value, (o) => CurrentSettings.AspectMode = value);
            }
        }

        public new float aspectRatio
        {
            get { return base.aspectRatio; }
            set
            {
                Config.Set(value, (o) => base.aspectRatio = value, (o) => CurrentSettings.AspectRatio = value);
            }
        }


        [SerializeField]
        private Settings settingsFallback = new Settings();

        [SerializeField]
        private SettingsConfigCollection customSettings = new SettingsConfigCollection();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Apply();
        }
        
        public void OnResolutionChanged()
        {
            Apply();
        }

        private void Apply()
        {
            base.aspectMode = CurrentSettings.AspectMode;
            base.aspectRatio = CurrentSettings.AspectRatio;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Apply();
        }
#endif
    }
    
}
