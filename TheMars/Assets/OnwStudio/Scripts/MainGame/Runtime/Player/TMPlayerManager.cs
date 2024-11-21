using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.Localization.Settings;
using Onw.Event;
using Onw.Helper;
using Onw.Manager;
using Onw.Attribute;
using TM.Event;
using TM.Manager;
using TM.Card.Runtime;

namespace TM
{
    public sealed class TMPlayerManager : SceneSingleton<TMPlayerManager>
    {
        public const int MAX_SATISFACTION = 100;
        public const int MAX_TERRAFORM_VALUE = 100;
        
        protected override string SceneName => "MainGameScene";
        
        public IReactiveField<int> MarsLithium => _marsLithium;
        public IReactiveField<int> Credit => _credit;
        public IReadOnlyReactiveField<int> Population => _population;
        public IReactiveField<int> TotalPopulation => _totalPopulation;
        public IReactiveField<int> Steel => _steel;
        public IReactiveField<int> Plants => _plants;
        public IReactiveField<int> Clay => _clay;
        public IReactiveField<int> Electricity => _electricity;
        public IReactiveField<int> Satisfaction => _satisfaction;

        public IReactiveField<int> Level => _level;
        public IReadOnlyReactiveField<int> Exp => _exp;
        public IReactiveField<int> MaxExp => _maxExp;
        public IReactiveField<int> TerraformValue => _terraformValue;

        [Header("Resources")]
        [SerializeField] private ReactiveField<int> _marsLithium = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _credit = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _population = new();
        [SerializeField] private ReactiveField<int> _totalPopulation = new() { ValueProcessors = new() { new MinIntProcessor(5) }};
        [SerializeField] private ReactiveField<int> _steel = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _plants = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _clay = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _electricity = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _satisfaction = new() { Value = 50, ValueProcessors = new() { new ClampIntProcessor(0, MAX_SATISFACTION)}};
        
        [Header("Player Info")]
        [SerializeField] private ReactiveField<int> _level = new() { Value = 1, ValueProcessors = new() { new MinIntProcessor(1) }};
        [SerializeField] private ReactiveField<int> _exp = new() { ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _maxExp = new() { Value = 100, ValueProcessors = new() { new MinIntProcessor(0) }};
        [SerializeField] private ReactiveField<int> _terraformValue = new() { ValueProcessors = new() { new ClampIntProcessor(0, MAX_TERRAFORM_VALUE) }};

        public void SetPopulation(int population)
        {
            _population.Value = Mathf.Clamp(population, 0, _totalPopulation.Value);
        }

        protected override void Init()
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            TMCardManager.Instance.CardCreator.OnPostCreateCard += onPostCreateCard;
            TMEventManager.Instance.OnTriggerEvent += onTriggerEvent;
        }

        private void onTriggerEvent(ITMEventRunner runner)
        {
            SetExp(_exp.Value + 10);
        }

        private void onPostCreateCard(TMCardModel card)
        {
            SetExp(_exp.Value + 10);
        }

        private void onChangedDay(int day)
        {
            if (day == 0) return;

            SetExp(_exp.Value + 5);
        }

        /// <summary>
        /// .. Exp의 경우 _maxExp에 의해 값이 유동적으로 변경되므로 .. 별도의 세터 메서드 제공
        /// </summary>
        /// <param name="exp"></param>
        public void SetExp(int exp)
        {
            if (exp < 0)
            {
                exp = 0;
            }

            if (exp >= _maxExp.Value)
            {
                exp -= _maxExp.Value;
                _level.Value++;
            }

            _exp.Value = exp;
        }

        public int GetResourceByKind(TMResourceKind kind)
        {
            return kind switch
            {
                TMResourceKind.MARS_LITHIUM => _marsLithium.Value,
                TMResourceKind.CREDIT => _credit.Value,
                TMResourceKind.POPULATION => _population.Value,
                TMResourceKind.STEEL => _steel.Value,
                TMResourceKind.PLANT => _plants.Value,
                TMResourceKind.CLAY => _clay.Value,
                TMResourceKind.ELECTRICITY => _electricity.Value,
                TMResourceKind.SATISFACTION => _satisfaction.Value,
                _ => 0,
            };
        }
            
        public void AddResource(TMResourceKind kind, int resource)
        {
            switch (kind)
            {
                case TMResourceKind.MARS_LITHIUM:
                    _marsLithium.Value += resource;
                    break;
                case TMResourceKind.CREDIT:
                    _credit.Value += resource;
                    break;
                case TMResourceKind.STEEL:
                    _steel.Value += resource;
                    break;
                case TMResourceKind.PLANT:
                    _plants.Value += resource;
                    break;
                case TMResourceKind.CLAY:
                    _clay.Value += resource;
                    break;
                case TMResourceKind.ELECTRICITY:
                    _electricity.Value += resource;
                    break;
                case TMResourceKind.POPULATION:
                    SetPopulation(_population.Value + resource);
                    break;
                case TMResourceKind.SATISFACTION:
                    _satisfaction.Value += resource;
                    break;
            }
        }
    }
}