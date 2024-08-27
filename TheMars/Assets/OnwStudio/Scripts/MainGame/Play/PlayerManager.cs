using UnityEngine;
using Onw.Attribute;
using Onw.Manager;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TM
{
    public sealed class PlayerManager : SceneSingleton<PlayerManager>
    {
        public override string SceneName => "MainGameScene";

        public int Tera
        {
            get => _tera;
            set
            {
                _tera = value;

                if (_tera < 0)
                {
                    _tera = 0;
                }
            }
        }

        public int MarsLithium
        {
            get => _marsLithium;
            set
            {
                _marsLithium = value;

                if (_marsLithium < 0)
                {
                    _marsLithium = 0;
                }
            }
        }

        [SerializeField, ReadOnly] private int _marsLithium = 0;
        [SerializeField, ReadOnly] private int _tera = 0;

        protected override void Init()
        {
            Locale locale = LocalizationSettings.AvailableLocales.GetLocale("ko-KR");
            if (locale)
            {
                LocalizationSettings.SelectedLocale = locale;
            }
            else
            {
                Debug.LogWarning("선택한 로케일을 찾을 수 없습니다: " + "ko-KR");
            }
        }
    }
}
