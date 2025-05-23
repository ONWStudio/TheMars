using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Manager;
using Onw.Prototype;
using Onw.ObjectPool;
using TM.UI;
using TM.Manager;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;

namespace TM.Runtime
{
    public sealed class TMCardNotifyIconSpawner : SceneSingleton<TMCardNotifyIconSpawner>
    {
        private const float REPEAT_TIME_MIN = 5f;
        private const float REPEAT_TIME_MAX = 10f;

        protected override string SceneName => "MainGameScene";

        public event UnityAction<TMCardCollectNotifyIcon> OnCreateIcon
        {
            add => _onCreateIcon.AddListener(value);
            remove => _onCreateIcon.RemoveListener(value);
        }

        [field: SerializeField] public int CardCreationCount { get; set; } = 3;

        [SerializeField, Range(REPEAT_TIME_MIN, REPEAT_TIME_MAX)] private float _repeatTime = 5f;

        [SerializeField] private UnityEvent<TMCardCollectNotifyIcon> _onCreateIcon = new();

        protected override void Init()
        {
        }

        private void Start()
        {
            TMSimulator.Instance.NowDay.AddListener(onChangedDay);

            void onChangedDay(int day)
            {
                CreateIcon(TMCardManager
                    .Instance
                    .CardCreator
                    .CreateRandomCardsByWhere(cardData => cardData.CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>(), CardCreationCount));
            }
        }

        public void CreateIcon(TMCardModel[] cards)
        {
            if (cards is null)
            {
                Debug.LogError("TMCardNotifyIconSpawner : 만들어진 카드가 0개입니다");
                return;
            }

            if (GenericObjectPool<TMCardCollectNotifyIcon>.TryPop(out TMCardCollectNotifyIcon iconInstance))
            {
                onLoadedNotifyIcon(iconInstance);
            }
            else
            {
                PrototypeManager.Instance.ClonePrototypeAsync<TMCardCollectNotifyIcon>("Card_Collect_Notify_Icon", onLoadedNotifyIcon);
            }

            void onLoadedNotifyIcon(TMCardCollectNotifyIcon icon)
            {
                icon.SetCards(cards.ToList());
                _onCreateIcon.Invoke(icon);

                TMCardManager.Instance.UIComponents.CardCollectIconScrollView.AddItem(icon.gameObject);
            }
        }
    }
}