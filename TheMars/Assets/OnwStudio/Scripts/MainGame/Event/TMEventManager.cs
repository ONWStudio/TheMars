using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Onw.Event;
using Onw.Manager;
using Onw.Attribute;
using Onw.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using TM.Manager;
using TM.Card.Runtime;
using TM.Event.Effect;
using JetBrains.Annotations;

namespace TM.Event
{
    public interface ITMEventProbability
    {
        IReactiveField<int> DefaultProbability { get; }
        IReactiveField<int> AdditionalProbability { get; }
        IReactiveField<bool> IsStatic { get; }
        
        int FinalProbability { get; }
    }
    
    [System.Serializable]
    public class TMEventProbability : ITMEventProbability
    {
        public IReactiveField<int> DefaultProbability => ReactDefaultProbability;
        public IReactiveField<int> AdditionalProbability => ReactAdditionalProbability;
        public IReactiveField<bool> IsStatic => ReactIsStatic;

        [field: SerializeField] public ReactiveField<int> ReactDefaultProbability { get; private set; } = new() { ValueProcessors = new() { new ClampIntProcessor(0, 100) }};
        [field: SerializeField] public ReactiveField<int> ReactAdditionalProbability { get; private set; } = new();
        [field: SerializeField] public ReactiveField<bool> ReactIsStatic { get; private set; } = new();

        public int FinalProbability => ReactIsStatic.Value ? ReactDefaultProbability.Value : Mathf.Clamp(ReactDefaultProbability.Value + ReactAdditionalProbability.Value, 0, 100);
    }
    
    public sealed class TMEventManager : SceneSingleton<TMEventManager>
    {
        public event UnityAction<ITMEventRunner> OnTriggerEvent
        {
            add => _onTriggerEvent.AddListener(value);
            remove => _onTriggerEvent.RemoveListener(value);
        }

        private readonly Queue<TMEventRunner> _eventQueue = new();

        [Header("Event Probability")]
        [SerializeField] private TMEventProbability _calamityEventProbability = new();
        [SerializeField] private TMEventProbability _positiveEventProbability = new();
        [SerializeField] private TMEventProbability _negativeEventProbability = new();

        [Header("Unique Events")]
        [SerializeField, ReadOnly] private TMEventRunner _mainEventRunner = null;
        [SerializeField, ReadOnly] private TMEventRunner _marsLithiumEventRunner = null;

        [FormerlySerializedAs("_onTriggerMainEvent")]
        [Header("Event")]
        [SerializeField] private UnityEvent<ITMEventRunner> _onTriggerEvent = new();

        protected override string SceneName => "MainGameScene";

        public ITMEventRunner MainEventRunner => _mainEventRunner;
        public ITMEventRunner MarsLithiumEventRunner => _marsLithiumEventRunner;
        
        [field: SerializeField, OnwMin(1)] public int CheckMainEventLevelCount { get; private set; } = 10;
        [field: SerializeField, OnwMin(1)] public int CheckMarsLithiumEventDayCount { get; private set; } = 5;
        [field: SerializeField, OnwMin(1)] public int CheckCalamityEventDayCount { get; private set; } = 1;
        [field: SerializeField, OnwMin(1)] public int CheckRegularEventDayCount { get; private set; } = 1;
       
        public ITMEventProbability CalamityEventProbability => _calamityEventProbability;
        public ITMEventProbability PositiveEventProbability => _positiveEventProbability;
        public ITMEventProbability NegativeEventProbability => _negativeEventProbability;

        protected override void Init()
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            TMPlayerManager.Instance.Level.AddListener(onChangedLevel);
            TMCardManager.Instance.CardCreator.OnPostCreateCard += onPostCreateCard;
            _mainEventRunner = new(TMEventDataManager.Instance.RootMainEventData);
            _marsLithiumEventRunner = new(TMEventDataManager.Instance.MarsLithiumEvent);
            _positiveEventProbability.DefaultProbability.Value = 20;
            _negativeEventProbability.DefaultProbability.Value = 20;

            _eventQueue.Enqueue(_mainEventRunner);
            _eventQueue.Enqueue(_marsLithiumEventRunner);

            fireEvents();
        }

        private void onPostCreateCard(TMCardModel card)
        {
            card.OnSellCard += onSellCard;

            void onSellCard(TMCardModel _)
            {
                CalamityEventProbability.AdditionalProbability.Value++;
            }
        }

        private void onChangedLevel(int level)
        {
            handleMainEvent(level);
        }

        private void handleMainEvent(int level)
        {
            if (level % CheckMainEventLevelCount != 0 || _mainEventRunner is null) return;
            
            TMEventRunner mainEventRunner = _mainEventRunner;
            mainEventRunner.OnFireEvent += onFireMainEvent;

            _eventQueue.Enqueue(mainEventRunner);
            _mainEventRunner = null;

            void onFireMainEvent(TMEventChoice eventChoice)
            {
                mainEventRunner.OnFireEvent -= onFireMainEvent;
                _mainEventRunner = new(eventChoice == TMEventChoice.TOP ?
                    mainEventRunner.EventData.TopEventData :
                    mainEventRunner.EventData.BottomEventData);
            }
        }

        // .. TODO : 이벤트 코스트, 효과등 적용되지 않는 버그? (UI 업데이트가 적용되지 않는걸로 추정)
        private void handleMarsLithiumEvent(int day)
        {
            if (day % CheckMarsLithiumEventDayCount != 0 || _marsLithiumEventRunner is null || !_marsLithiumEventRunner.EventData) return;

            _eventQueue.Enqueue(_marsLithiumEventRunner);
        }

        private void handleCalamityEvent(int day)
        {
            if (day % CheckCalamityEventDayCount != 0 ||
                TMEventDataManager.Instance.CalamityEventList.Count <= 0 ||
                CalamityEventProbability.FinalProbability <= Random.Range(0, 100)) return;

            TMEventRunner calamityEvent = new(
                TMEventDataManager.Instance.CalamityEventList[Random.Range(0, TMEventDataManager.Instance.CalamityEventList.Count)]);

            _eventQueue.Enqueue(calamityEvent);
        }

        private void handleRegularEvent(int day)
        {
            if (day % CheckRegularEventDayCount != 0) return;

            const int SATISFACTION_HALF = TMPlayerManager.MAX_SATISFACTION / 2;

            switch (TMPlayerManager.Instance.Satisfaction.Value)
            {
                case var satisfaction and > SATISFACTION_HALF:
                    creationPositiveEvent(satisfaction);
                    break;
                case var satisfaction and < SATISFACTION_HALF:
                    creationNegativeEvent(satisfaction);
                    break;
                case var satisfaction:
                    if (Random.Range(0, 2) == 0)
                    {
                        creationPositiveEvent(satisfaction);
                    }
                    else
                    {
                        creationNegativeEvent(satisfaction);
                    }
                    break;
            }

            void creationPositiveEvent(int satisfaction)
            {
                if (TMEventDataManager.Instance.PositiveEventList.Count <= 0) return;

                int satisfactionHalfCalc = SATISFACTION_HALF - satisfaction;
                int probability = PositiveEventProbability.IsStatic.Value ? 
                    PositiveEventProbability.DefaultProbability.Value : 
                    PositiveEventProbability.FinalProbability + (satisfactionHalfCalc == 0 ? 0 : (SATISFACTION_HALF / satisfactionHalfCalc + 1) * 10);
                
                if (probability > Random.Range(0, 100))
                {
                    TMEventRunner positiveEvent = new(
                        TMEventDataManager.Instance.PositiveEventList[Random.Range(0, TMEventDataManager.Instance.PositiveEventList.Count)]);

                    _eventQueue.Enqueue(positiveEvent);
                }
            }

            void creationNegativeEvent(int satisfaction)
            {
                if (TMEventDataManager.Instance.NegativeEventList.Count <= 0) return;

                int satisfactionHalfCalc = SATISFACTION_HALF - satisfaction;
                int probability = NegativeEventProbability.IsStatic.Value ?
                    NegativeEventProbability.DefaultProbability.Value :
                    NegativeEventProbability.FinalProbability + (satisfactionHalfCalc == 0 ? 0 : (SATISFACTION_HALF / satisfactionHalfCalc + 1) * 10);
                
                if (probability > Random.Range(0, 100))
                {
                    TMEventRunner negativeEvent = new(
                        TMEventDataManager.Instance.NegativeEventList[Random.Range(0, TMEventDataManager.Instance.NegativeEventList.Count)]);

                    _eventQueue.Enqueue(negativeEvent);
                }
            }
        }

        private void onChangedDay(int day)
        {
            if (day == 1) return;

            handleMarsLithiumEvent(day);
            handleCalamityEvent(day);
            handleRegularEvent(day);

            fireEvents();
        }

        // .. TODO : Event Queue 추가할때 이벤트가 이미 실행중인지 판별 로직 작성해야함
        private void fireEvents()
        {
            if (_eventQueue.Count <= 0) return;

            bool keepPause = TimeManager.IsPause;
            TimeManager.IsPause = true;

            TMEventRunner selectEvent = _eventQueue.Dequeue();
            selectEvent.OnFireEvent += onEndEvent;
            _onTriggerEvent.Invoke(selectEvent);

            void onEndEvent(TMEventChoice eventChoice)
            {
                Debug.Log("?");
                selectEvent.OnFireEvent -= onEndEvent;
                TimeManager.IsPause = keepPause;
                fireEvents();
            }
        }
    }
}