using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.ObjectPool;
using Onw.Components.Movement;
using TM.Card.Runtime;

namespace TM.UI
{
    public sealed class TMCardCollectNotifyIcon : MonoBehaviour, IReturnHandler
    {
        public event UnityAction OnNotifyCantCreateCard
        {
            add => _onNotifyCantCreateCard.AddListener(value);
            remove => _onNotifyCantCreateCard.RemoveListener(value);
        }
        
        [field: SerializeField] public float DestroyTime { get; private set; } = 60f;
        
        [Header("Button")]
        [SerializeField, InitializeRequireComponent] private Button _collectCardButton;
        
        [Header("Option")]
        [SerializeField, InitializeRequireComponent] private Vector2SmoothMover _smoothMover;

        [SerializeField, ReadOnly] private List<TMCardModel> _cards = new();

        [Header("Notify")]
        [SerializeField] private UnityEvent _onNotifyCantCreateCard = new();
        
        private void Start()
        {
            _smoothMover.IsLocal = true;
            
            _collectCardButton.onClick.AddListener(() =>
            {
                if (TMCardManager.Instance.Cards.Count >= TMCardManager.Instance.MaxCardCount.Value)
                {
                    _onNotifyCantCreateCard.Invoke();
                    return;
                }
                
                TMCardCollectUIController.Instance.ActiveUI(_cards);
                GenericObjectPool<TMCardCollectNotifyIcon>.Return(this);
            });
        }

        public void SetCards(List<TMCardModel> cards)
        {
            _cards.Clear();
            _cards.AddRange(cards);
        }
        
        public void SetTargetLocalPosition(Vector3 targetPosition)
        {
            _smoothMover.TargetPosition = targetPosition;
        }
        
        public void OnReturnToPool()
        {
            _cards.Clear();
        }
    }
}
