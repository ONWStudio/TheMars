using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using Onw.ServiceLocator;
using TMCard.Runtime;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace TMCard.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardCollectUI : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private RectTransform _cardSelectField;
        [SerializeField, SelectableSerializeField] private Button _selectButton;

        private readonly List<KeyValuePair<TMCardController, Action<PointerEventData>>> _cardCallbacks = new();
        private TMCardController _selectCard = null;
        
        private void Start()
        {
            _selectButton.onClick.AddListener(() =>
            {
                if (!_selectCard || !ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;

                foreach (KeyValuePair<TMCardController, Action<PointerEventData>> callbackPair in _cardCallbacks)
                {
                    callbackPair.Key.InputHandler.RemoveListenerPointerClickAction(callbackPair.Value);

                    if (callbackPair.Key != _selectCard)
                    {
                        Destroy(callbackPair.Key.gameObject);
                    }
                }

                _cardCallbacks.Clear();

                _selectCard.SmoothMove.enabled = true;
                _selectCard.transform.localScale = new(1f, 1f, 1f);
                service.CardHandController.AddCard(_selectCard);
                service.FeedbackPlayer.EnqueueEvent(
                    _selectCard.GetMoveToScreenCenterEvent(),
                    service.CardHandController.GetSortCardsFeedbacks());

                _selectCard = null;
                gameObject.SetActive(false);
            });
        }

        public void ActiveUI()
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;
            
            gameObject.SetActive(true);
            
            foreach (TMCardController card in service.CardCreator.CreateCards(3))
            {
                card.transform.SetParent(_cardSelectField, false);
                
                Action<PointerEventData> action = selectCard;
                _cardCallbacks.Add(new(card, action));
                card.InputHandler.AddListenerPointerClickAction(action);
                card.SmoothMove.enabled = false;

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
