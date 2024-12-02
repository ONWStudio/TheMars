using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Onw.Attribute;
using Onw.Components;
using Onw.Manager;
using TM.Card.Runtime;
using TM.Manager;

namespace TM.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardCollectUIController : SceneSingleton<TMCardCollectUIController>
    {
        public event UnityAction<TMCardModel> OnAddedCardEvent
        {
            add => _onAddedCardEvent.AddListener(value);
            remove => _onAddedCardEvent.RemoveListener(value);
        }
        public event UnityAction<TMCardModel> OnRemovedCardEvent
        {
            add => _onRemovedCardEvent.AddListener(value);
            remove => _onRemovedCardEvent.RemoveListener(value);
        }

        protected override string SceneName => "MainGameScene";

        [SerializeField] private UnityEvent<TMCardModel> _onAddedCardEvent = new();
        [SerializeField] private UnityEvent<TMCardModel> _onRemovedCardEvent = new();

        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private RectTransform _cardSelectField;
        [SerializeField, SelectableSerializeField] private Button _selectButton;
        
        private readonly List<KeyValuePair<TMCardModel, UnityAction<PointerEventData>>> _cardCallbacks = new();
        private readonly List<TMCardModel> _cards = new();
        private TMCardModel _selectCard = null;
        private bool? _keepPause = null; 

        protected override void Init() {}

        private void Start()
        {
            _selectButton.onClick.AddListener(() =>
            {
                if (!_selectCard) return;

                foreach (KeyValuePair<TMCardModel, UnityAction<PointerEventData>> callbackPair in _cardCallbacks)
                {
                    callbackPair.Key.PointerUpTrigger.OnPointerUpEvent -= callbackPair.Value;

                    if (callbackPair.Key != _selectCard)
                    {
                        Destroy(callbackPair.Key.gameObject);
                    }
                }

                _cards.ForEach(_onRemovedCardEvent.Invoke);

                TimeManager.IsPause = _keepPause is not null && (bool)_keepPause;
                _keepPause = null;
                _cardCallbacks.Clear();
                _selectCard.Initialize();
                _selectCard.transform.localScale = new(1f, 1f, 1f);
                _selectCard.SetCanInteract(true);
                TMCardManager.Instance.AddCard(_selectCard);
                _selectCard = null;
                _canvas.enabled = false;
                _cards.Clear();
            });
        }

        public void ActiveUI(List<TMCardModel> cards)
        {
            _canvas.enabled = true;
            _keepPause = TimeManager.IsPause;
            TimeManager.IsPause = true;
            _cards.AddRange(cards);

            foreach (TMCardModel card in cards)
            {
                _onAddedCardEvent.Invoke(card);
                card.transform.SetParent(_cardSelectField, false);
                card.SetCanInteract(false);
                UnityAction<PointerEventData> action = selectCard;
                _cardCallbacks.Add(new(card, action));
                card.PointerUpTrigger.OnPointerUpEvent += action;
                
                void selectCard(PointerEventData _)
                {
                    if (_selectCard)
                    {
                        _selectCard.transform.localScale = new(1f, 1f, 1f);
                        
                        if (_selectCard == card)
                        {
                            _selectCard = null;
                        }
                        else
                        {
                            _selectCard = card;
                            _selectCard.transform.localScale = new Vector3(_selectCard.transform.localScale.x, _selectCard.transform.localScale.y, 0f) * 1.25f;
                        }
                    }
                    else
                    {
                        _selectCard = card;
                        _selectCard.transform.localScale = new Vector3(_selectCard.transform.localScale.x, _selectCard.transform.localScale.y, 0f) * 1.25f;
                    }
                }
            }
        }

    }
}
