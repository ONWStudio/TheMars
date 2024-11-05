using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UniRx;
using UniRx.Triggers;
using Onw.Attribute;

namespace TM.Card.Runtime
{
    // .. Controller
    [DisallowMultipleComponent]
    public sealed class TMCardController : MonoBehaviour
    {
        public TMCardData CardData { get; set; } = null;

        [Header("Model")]
        [SerializeField, InitializeRequireComponent]
        private TMCardModel _cardModel;

        [Header("View")]
        [SerializeField, InitializeRequireComponent]
        private TMCardViewer _cardViewer;

        private void Start()
        {
            if (_cardModel.CardData)
            {
                setCard(_cardModel.CardData);
            }
            else
            {
                _cardModel
                    .ObserveEveryValueChanged(cardModel => cardModel.CardData)
                    .Take(1)
                    .Subscribe(setCard)
                    .AddTo(this);
            }
            
            _cardModel
                .ObserveEveryValueChanged(cardModel => cardModel.IsHide)
                .Subscribe(isHide => _cardViewer.SetView(!isHide))
                .AddTo(this);

            void setCard(TMCardData cardData)
            {
                CardData = _cardModel.CardData;
                _cardViewer.SetUI(_cardModel);
            }
        }
    }
}
