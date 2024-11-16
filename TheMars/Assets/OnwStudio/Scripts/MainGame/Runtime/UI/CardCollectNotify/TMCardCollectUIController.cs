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

namespace TM.Runtime.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardCollectUIController : SceneSingleton<TMCardCollectUIController>
    {
        protected override string SceneName => "MainGameScene";
        
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private RectTransform _cardSelectField;
        [SerializeField, SelectableSerializeField] private Button _selectButton;
        
        private readonly List<KeyValuePair<TMCardModel, UnityAction<PointerEventData>>> _cardCallbacks = new();
        private TMCardModel _selectCard = null;

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

                TimeManager.IsPause = false;
                _cardCallbacks.Clear();
                _selectCard.Initialize();
                _selectCard.transform.localScale = new(1f, 1f, 1f);
                _selectCard.SetCanInteract(true);
                TMCardManager.Instance.AddCard(_selectCard);
                _selectCard = null;
                _canvas.enabled = false;
            });
        }

        public void ActiveUI(List<TMCardModel> cards)
        {
            _canvas.enabled = true;
            TimeManager.IsPause = true;
            
            foreach (TMCardModel card in cards)
            {
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
