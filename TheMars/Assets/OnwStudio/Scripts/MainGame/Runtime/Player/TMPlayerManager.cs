using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Onw.Helper;
using Onw.Manager;
using Onw.Attribute;

namespace TM
{
    public sealed class TMPlayerManager : SceneSingleton<TMPlayerManager>
    {
        public override string SceneName => "MainGameScene";
        
        public int Level
        {
            get => _level;
            set
            {
                _level = DefaultValueHelper.Min(value, 0);
                _onChangedLevel.Invoke(_level);
            }
        }

        public int MarsLithium
        {
            get => _marsLithium;
            set
            {
                _marsLithium = DefaultValueHelper.Min(value, 0);
                _onChangedMarsLithium.Invoke(_marsLithium);
            }
        }

        public int Credit
        {
            get => _credit;
            set
            {
                _credit = DefaultValueHelper.Min(value, 0);
                _onChangedCredit.Invoke(_credit);
            }
        }

        public int Population
        {
            get => _population;
            set
            {
                _population = Mathf.Clamp(value, 0, _totalPopulation);
                _onChangedPopulation.Invoke(_population);
            }
        }

        public int TotalPopulation
        {
            get => _totalPopulation;
            set
            {
                _totalPopulation = DefaultValueHelper.Min(value, 5);
                _onChangedTotalPopulation.Invoke(_totalPopulation);
            }
        }

        public int Steel
        {
            get => _steel;
            set
            {
                _steel = DefaultValueHelper.Min(value, 0);
                _onChangedSteel.Invoke(_steel);
            }
        }

        public int Plants
        {
            get => _plants;
            set
            {
                _plants = DefaultValueHelper.Min(value, 0);
                _onChangedPlants.Invoke(_plants);
            }
        }

        public int Clay
        {
            get => _clay;
            set
            {
                _clay = DefaultValueHelper.Min(value, 0);
                _onChangedClay.Invoke(_clay);
            }
        }

        public int Electricity
        {
            get => _electricity;
            set
            {
                _electricity = DefaultValueHelper.Min(value, 0);
                _onChangedElectricity.Invoke(_electricity);
            }
        }

        public event UnityAction<int> OnChangedLevel
        {
            add => _onChangedLevel.AddListener(value);
            remove => _onChangedLevel.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedMarsLithium
        {
            add => _onChangedMarsLithium.AddListener(value);
            remove => _onChangedMarsLithium.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedCredit
        {
            add => _onChangedCredit.AddListener(value);
            remove => _onChangedCredit.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedPopulation
        {
            add => _onChangedPopulation.AddListener(value);
            remove => _onChangedPopulation.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedTotalPopulation
        {
            add => _onChangedTotalPopulation.AddListener(value);
            remove => _onChangedTotalPopulation.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedSteel
        {
            add => _onChangedSteel.AddListener(value);
            remove => _onChangedSteel.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedPlants
        {
            add => _onChangedPlants.AddListener(value);
            remove => _onChangedPlants.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedClay
        {
            add => _onChangedClay.AddListener(value);
            remove => _onChangedClay.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedElectricity
        {
            add => _onChangedElectricity.AddListener(value);
            remove => _onChangedElectricity.RemoveListener(value);
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

        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedMarsLithium = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedCredit = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedLevel = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedPopulation = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedTotalPopulation = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedSteel = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedPlants = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedClay = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedElectricity = new();

        public void AddResource(TMResourceKind kind, int resource)
        {
            switch (kind)
            {
                case TMResourceKind.MARS_LITHIUM:
                    MarsLithium += resource;
                    break;
                case TMResourceKind.CREDIT:
                    Credit += resource;
                    break;
                case TMResourceKind.STEEL:
                    Steel += resource;
                    break;
                case TMResourceKind.PLANTS:
                    Plants += resource;
                    break;
                case TMResourceKind.CLAY:
                    Clay += resource;
                    break;
                case TMResourceKind.ELECTRICITY:
                    Electricity += resource;
                    break;
                case TMResourceKind.POPULATION:
                    Population += resource;
                    break;
            }
        }

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