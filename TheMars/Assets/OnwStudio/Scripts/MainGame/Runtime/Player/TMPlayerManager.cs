using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.Localization.Settings;
using Onw.Helper;
using Onw.Manager;
using Onw.Attribute;
using Onw.Event;

namespace TM
{
    public sealed class TMPlayerManager : SceneSingleton<TMPlayerManager>
    {
        public const int MAX_SATISFACTION = 100;
        public const int MAX_TERRAFORM_VALUE = 100;
        
        protected override string SceneName => "MainGameScene";
        
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

        public int Satisfaction
        {
            get => _satisfaction;
            set
            {
                _satisfaction = Mathf.Clamp(value, 0, MAX_SATISFACTION);
                _onChangedSatisfaction.Invoke(_satisfaction);
            }
        }

        public int TerraformValue
        {
            get => _terraformValue;
            set
            {
                _terraformValue = Mathf.Clamp(value, 0, MAX_TERRAFORM_VALUE);
                _onChangedTerraformValue.Invoke(_terraformValue);
            }
        }

        public event UnityAction<int> OnChangedLevel
        {
            add
            {
                if (value is null) return;
                
                _onChangedLevel.AddListener(value);
                value.Invoke(_level);
            }
            remove => _onChangedLevel.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedMarsLithium
        {
            add
            {
                if (value is null) return;
                
                _onChangedMarsLithium.AddListener(value);
                value.Invoke(_marsLithium);
            }
            remove => _onChangedMarsLithium.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedCredit
        {
            add
            {
                if (value is null) return;
                
                _onChangedCredit.AddListener(value);
                value.Invoke(_credit);
            }
            remove => _onChangedCredit.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedPopulation
        {
            add
            {
                if (value is null) return;
                
                _onChangedPopulation.AddListener(value);
                value.Invoke(_population);
            }
            remove => _onChangedPopulation.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedTotalPopulation
        {
            add
            {
                if (value is null) return;
                
                _onChangedTotalPopulation.AddListener(value);
                value.Invoke(_totalPopulation);
            }
            remove => _onChangedTotalPopulation.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedSteel
        {
            add
            {
                if (value is null) return;

                _onChangedSteel.AddListener(value);
                value.Invoke(_steel);
            }
            remove => _onChangedSteel.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedPlants
        {
            add
            {
                if (value is null) return;
                
                _onChangedPlants.AddListener(value);
                value.Invoke(_plants);
            }
            remove => _onChangedPlants.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedClay
        {
            add
            {
                if (value is null) return;
                
                _onChangedClay.AddListener(value);
                value.Invoke(_clay);
            }
            remove => _onChangedClay.RemoveListener(value);
        }

        public event UnityAction<int> OnChangedElectricity
        {
            add
            {
                if (value is null) return;
                
                _onChangedElectricity.AddListener(value);
                value.Invoke(_electricity);
            }
            remove => _onChangedElectricity.RemoveListener(value);
        }
        
        public event UnityAction<int> OnChangedSatisfaction
        {
            add
            {
                if (value is null) return;
            
                _onChangedSatisfaction.AddListener(value);
                value.Invoke(_satisfaction);
            }
            remove => _onChangedSatisfaction.RemoveListener(value);
        }
        
        public event UnityAction<int> OnChangedTerraformValue
        {
            add
            {
                if (value is null) return;
                
                _onChangedTerraformValue.AddListener(value);
                value.Invoke(_terraformValue);
            }
            remove => _onChangedTerraformValue.RemoveListener(value);
        }

        [Header("Resources")]
        [SerializeField, ReadOnly] private int _marsLithium = 0;
        [SerializeField, ReadOnly] private int _credit = 0;
        [SerializeField, ReadOnly] private int _population = 0;
        [SerializeField, ReadOnly] private int _totalPopulation = 0;
        [SerializeField, ReadOnly] private int _steel = 0;
        [SerializeField, ReadOnly] private int _plants = 0;
        [SerializeField, ReadOnly] private int _clay = 0;
        [SerializeField, ReadOnly] private int _electricity = 0;
        [SerializeField, ReadOnly] private int _satisfaction = 0;
        
        [Header("Player Info")]
        [SerializeField, ReadOnly] private int _level = 1;
        [SerializeField, ReadOnly] private int _exp = 0;
        [SerializeField, ReadOnly] private int _terraformValue = 0;

        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedMarsLithium = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedCredit = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedLevel = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedPopulation = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedTotalPopulation = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedSteel = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedPlants = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedClay = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedElectricity = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedSatisfaction = new();
        [SerializeField, ReadOnly] private UnityEvent<int> _onChangedTerraformValue = new();
            
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