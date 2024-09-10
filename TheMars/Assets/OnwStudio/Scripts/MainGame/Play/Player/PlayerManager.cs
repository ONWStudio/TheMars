using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Onw.Attribute;
using Onw.Helper;
using Onw.ServiceLocator;

namespace TM
{
    public sealed class PlayerManager : MonoBehaviour
    {
        public int Credit
        {
            get => _credit;
            set => _credit = DefaultValueHelper.Min(value, 0);
        }

        public int MarsLithium
        {
            get => _marsLithium;
            set => _marsLithium = DefaultValueHelper.Min(value, 0);
        }

        public int Level
        {
            get => _level;
            set => _level = DefaultValueHelper.Min(value, 0);
        }

        public int Population
        {
            get => _population;
            set => _population = Mathf.Clamp(value, 0, _totalPopulation);
        }

        public int TotalPopulation
        {
            get => _totalPopulation;
            set => _totalPopulation = DefaultValueHelper.Min(value, 5);
        }

        public int Steel
        {
            get => _steel;
            set => _steel = DefaultValueHelper.Min(value, 0);
        }

        public int Plants
        {
            get => _plants;
            set => _plants = DefaultValueHelper.Min(value, 0);
        }

        public int Clay
        {
            get => _clay;
            set => _clay = DefaultValueHelper.Min(value, 0);
        }

        public int Electricity
        {
            get => _electricity;
            set => _electricity = DefaultValueHelper.Min(value, 0);
        }

        [SerializeField, ReadOnly] private int _marsLithium = 0;
        [SerializeField, ReadOnly] private int _credit = 0;
        [SerializeField, ReadOnly] private int _level = 0;
        [SerializeField, ReadOnly] private int _population = 0;
        [SerializeField, ReadOnly] private int _totalPopulation = 0;
        [SerializeField, ReadOnly] private int _steel = 0;
        [SerializeField, ReadOnly] private int _plants = 0;
        [SerializeField, ReadOnly] private int _clay = 0;
        [SerializeField, ReadOnly] private int _electricity = 0;
        
        private void Awake()
        {
            if (ServiceLocator<PlayerManager>.RegisterService(this)) return;
            
            ServiceLocator<PlayerManager>.ChangeService(this);
        }

        private void Start()
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
