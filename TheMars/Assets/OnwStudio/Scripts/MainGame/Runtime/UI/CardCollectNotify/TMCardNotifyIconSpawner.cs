using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Manager;
using Onw.Manager.ObjectPool;
using TM.Card.Effect.Creator;
using TM.Manager;
using TM.Runtime.UI;
using TM.Card.Runtime;

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
        
        [SerializeField] private TMCardCollectNotifyIcon _iconPrefab = null;
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
                Debug.LogError("");
            }
            
            if (!GenericObjectPool<TMCardCollectNotifyIcon>.TryPop(out TMCardCollectNotifyIcon iconInstance))
            {
                iconInstance = Instantiate(_iconPrefab.gameObject)
                    .GetComponent<TMCardCollectNotifyIcon>();
                    
                _onCreateIcon.Invoke(iconInstance);
                iconInstance.SetCards(cards.ToList());
            }
            
            TMCardManager.Instance.UIComponents.CardCollectIconScrollView.AddItem(iconInstance.gameObject);
        }
    }
}