using System;
using Onw.Attribute;
using Onw.ServiceLocator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
namespace TM.Card.Runtime
{
    // .. Controller
    [DisallowMultipleComponent]
    public sealed class TMCardController : MonoBehaviour
    {
        public TMCardData CardData
        {
            get => _cardData;
            set
            {
                _cardData = value;
                _cardViewer.SetUI(_cardData);
            }
        }
        
        [FormerlySerializedAs("_cardCardModel")]
        [FormerlySerializedAs("_cardController")]
        [Header("Model")]
        [SerializeField, InitializeRequireComponent]
        private TMCardModel _cardModel;

        [Header("View")]
        [SerializeField, InitializeRequireComponent]
        private TMCardViewer _cardViewer;

        private TMCardData _cardData;

        private void Start()
        {
            if (_cardModel.CardData)
            {
                CardData = _cardModel.CardData;
            }
            else
            {
                _cardModel
                    .ObserveEveryValueChanged(cardModel => cardModel.CardData)
                    .Take(1)
                    .Subscribe(cardData => CardData = cardData)
                    .AddTo(this);
            }
            
            _cardModel
                .ObserveEveryValueChanged(cardModel => cardModel.IsHide)
                .Subscribe(isHide => _cardViewer.SetView(!isHide))
                .AddTo(this);
        }
    }
}
