using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace TMCard.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardCollectUIController : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private RectTransform _cardSelectField;
        [SerializeField, SelectableSerializeField] private Button _selectButton;

        private readonly List<KeyValuePair<TMCardModel, UnityAction<PointerEventData>>> _cardCallbacks = new();
        private TMCardModel _selectCard = null;

        private void Awake()
        {
            if (ServiceLocator<TMCardCollectUIController>.RegisterService(this)) return;

            ServiceLocator<TMCardCollectUIController>.ChangeService(this);
        }
        
        private void Start()
        {
            _selectButton.onClick.AddListener(() =>
            {
                if (!_selectCard || !ServiceLocator<TMCardManager>.TryGetService(out TMCardManager service)) return;

                foreach (KeyValuePair<TMCardModel, UnityAction<PointerEventData>> callbackPair in _cardCallbacks)
                {
                    callbackPair.Key.InputHandler.DownAction.AddListener(callbackPair.Value);

                    if (callbackPair.Key != _selectCard)
                    {
                        Destroy(callbackPair.Key.gameObject);
                    }
                }

                _cardCallbacks.Clear();

                _selectCard.CardBodyMover.enabled = true;
                _selectCard.CardViewMover.enabled = true;
                _selectCard.transform.localScale = new(1f, 1f, 1f);
                service.AddCard(_selectCard);
                _selectCard = null;
                
                gameObject.SetActive(false);
            });
        }

        public void ActiveUI()
        {
            if (!ServiceLocator<TMCardManager>.TryGetService(out TMCardManager service)) return;
            
            gameObject.SetActive(true);
            
            foreach (TMCardModel card in service.CardCreator.CreateCards(3))
            {
                card.transform.SetParent(_cardSelectField, false);
                
                UnityAction<PointerEventData> action = selectCard;
                _cardCallbacks.Add(new(card, action));
                card.InputHandler.DownAction.AddListener(action);
                card.CardBodyMover.enabled = false;
                card.CardViewMover.enabled = false;
                
                void selectCard(PointerEventData data)
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
