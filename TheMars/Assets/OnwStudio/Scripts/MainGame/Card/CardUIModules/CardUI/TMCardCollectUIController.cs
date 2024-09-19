using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Onw.Attribute;
using TM.Card.Runtime;
using VContainer;

namespace TM.Card.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardCollectUIController : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private RectTransform _cardSelectField;
        [SerializeField, SelectableSerializeField] private Button _selectButton;
        [SerializeField, Inject] private TMCardManager _cardManager;
        
        private readonly List<KeyValuePair<TMCardModel, UnityAction<PointerEventData>>> _cardCallbacks = new();
        private TMCardModel _selectCard = null;

        private void Start()
        {
            _selectButton.onClick.AddListener(() =>
            {
                if (!_selectCard) return;

                foreach (KeyValuePair<TMCardModel, UnityAction<PointerEventData>> callbackPair in _cardCallbacks)
                {
                    callbackPair.Key.InputHandler.UpAction -= callbackPair.Value;

                    if (callbackPair.Key != _selectCard)
                    {
                        Destroy(callbackPair.Key.gameObject);
                    }
                }

                _cardCallbacks.Clear();

                _selectCard.CardBodyMover.enabled = true;
                _selectCard.CardViewMover.enabled = true;
                _selectCard.transform.localScale = new(1f, 1f, 1f);
                _selectCard.Initialize();
                _cardManager.AddCard(_selectCard);
                _selectCard = null;
                _canvas.enabled = false;
            });
        }

        public void ActiveUI()
        {
            _canvas.enabled = true;
            
            foreach (TMCardModel card in _cardManager.CardCreator.CreateCards(3, false))
            {
                card.transform.SetParent(_cardSelectField, false);
                card.CardViewMover.enabled = false;
                card.CardBodyMover.enabled = false;
                
                UnityAction<PointerEventData> action = selectCard;
                _cardCallbacks.Add(new(card, action));
                card.InputHandler.UpAction += action;
                
                void selectCard(PointerEventData data)
                {
                    Debug.Log("Select");
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
