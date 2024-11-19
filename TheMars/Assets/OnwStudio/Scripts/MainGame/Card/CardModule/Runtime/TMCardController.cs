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
            if (_cardModel.CardData.Value)
            {
                setCard(_cardModel.CardData.Value);
            }
            else
            {
                _cardModel.CardData.AddListener(setCard);
            }

            _cardModel.IsHide.AddListener(isHide => _cardViewer.SetView(!isHide));

            void setCard(TMCardData cardData)
            {
                if (!cardData) return;

                _cardModel.CardData.RemoveListener(setCard);
                CardData = _cardModel.CardData.Value;
                _cardViewer.SetUI(_cardModel);
            }
        }
    }
}
