using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Manager;
using TM.Manager;
using TM.Card.Runtime;

namespace TM.Event
{
    public sealed class TMEventManager : SceneSingleton<TMEventManager>
    {
        protected override string SceneName => "MainGameScene";

        public event UnityAction<TMEventRunner> OnTriggerMainEvent
        {
            add => _onTriggerEvent.AddListener(value);
            remove => _onTriggerEvent.RemoveListener(value);
        }

        [field: SerializeField, OnwMin(1)] public int CheckMainEventLevelCount { get; private set; } = 10;
        [field: SerializeField, OnwMin(1)] public int CheckMarsLithiumEventDayCount { get; private set; } = 5;
        [field: SerializeField, OnwMin(1)] public int CheckCalamityEventDayCount { get; private set; } = 1;
        [field: SerializeField, OnwMin(1)] public int CheckRegularEventDayCount { get; private set; } = 1;
        [field: SerializeField, OnwMin(0), OnwMax(100)] public int CalamityEventProbability { get; private set; } = 0;
        [field: SerializeField, OnwMin(0), OnwMax(100)] public int DefaultPositiveEventProbability { get; private set; } = 20;
        [field: SerializeField, OnwMin(0), OnwMax(100)] public int DefaultNegativeEventProbability { get; private set; } = 20;

        [FormerlySerializedAs("_mainEvent")]
        [SerializeField] private TMEventRunner _mainEventRunner = null;

        [FormerlySerializedAs("_onTriggerMainEvent")]
        [Header("Event")]
        [SerializeField] private UnityEvent<TMEventRunner> _onTriggerEvent = new();

        private bool _isApplicationQuit = false;

        private readonly Queue<TMEventRunner> _eventQueue = new();

        protected override void Init()
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);
            TMPlayerManager.Instance.Level.AddListener(onChangedLevel);
            _mainEventRunner = new(TMEventDataManager.Instance.RootMainEventData);
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
                TimeManager.IsPause = false;
                mainEventRunner.OnFireEvent -= onFireMainEvent;
                _mainEventRunner = new(eventChoice == TMEventChoice.TOP ?
                    mainEventRunner.EventData.TopEventData :
                    mainEventRunner.EventData.BottomEventData);
            }
        }

        private void handleMarsLithiumEvent(int day)
        {
            if (day % CheckMarsLithiumEventDayCount != 0 || !TMEventDataManager.Instance.MarsLithiumEvent) return;
            
            _eventQueue.Enqueue(new(TMEventDataManager.Instance.MarsLithiumEvent));
        }

        private void handleCalamityEvent(int day)
        {
            if (day % CheckCalamityEventDayCount != 0 ||
                TMEventDataManager.Instance.CalamityEventList.Count <= 0 ||
                CalamityEventProbability < Random.Range(0, 101)) return;

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

                int probability = DefaultPositiveEventProbability + (SATISFACTION_HALF / (satisfaction - SATISFACTION_HALF) + 1) * 10;
                Debug.Log("Positive Probability : " + probability);
                if (probability >= Random.Range(0, 101))
                {
                    TMEventRunner positiveEvent = new(
                        TMEventDataManager.Instance.PositiveEventList[Random.Range(0, TMEventDataManager.Instance.PositiveEventList.Count)]);

                    _eventQueue.Enqueue(positiveEvent);
                }
            }

            void creationNegativeEvent(int satisfaction)
            {
                if (TMEventDataManager.Instance.NegativeEventList.Count <= 0) return;

                int probability = DefaultNegativeEventProbability + (SATISFACTION_HALF / (SATISFACTION_HALF - satisfaction) + 1) * 10;
                Debug.Log("Negative Probability : " + probability);
                if (probability >= Random.Range(0, 101))
                {
                    TMEventRunner negativeEvent = new(
                        TMEventDataManager.Instance.NegativeEventList[Random.Range(0, TMEventDataManager.Instance.NegativeEventList.Count)]);

                    _eventQueue.Enqueue(negativeEvent);
                }
            }
        }

        private void onChangedDay(int day)
        {
            if (day == 0) return;

            handleMarsLithiumEvent(day);
            handleCalamityEvent(day);
            handleRegularEvent(day);

            fireEvents();
        }

        // .. TODO : Event Queue 추가할때 이벤트가 이미 실행중인지 판별 로직 작성해야함
        private void fireEvents()
        {
            if (_eventQueue.Count <= 0) return;

            TMEventRunner selectEvent = _eventQueue.Dequeue();
            TimeManager.IsPause = true;
            selectEvent.OnFireEvent += onEndEvent;
            _onTriggerEvent.Invoke(selectEvent);

            void onEndEvent(TMEventChoice eventChoice)
            {
                selectEvent.OnFireEvent -= onEndEvent;
                fireEvents();
            }
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        private void OnDestroy()
        {
            if (_isApplicationQuit) return;

            TMSimulator.Instance.NowDay.RemoveListener(onChangedDay);
        }
    }
}