using System;
using Onw.Attribute;
using Onw.ServiceLocator;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
namespace TMCard.Runtime
{
    // .. Controller
    [DisallowMultipleComponent]
    public sealed class TMCardController : MonoBehaviour
    {
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
                SetCardData(_cardModel.CardData);
            }
            else
            {
                _cardModel
                    .ObserveEveryValueChanged(cardModel => cardModel.CardData)
                    .Take(1)
                    .Subscribe(SetCardData)
                    .AddTo(this);
            }
            
            _cardModel
                .ObserveEveryValueChanged(cardModel => cardModel.IsHide)
                .Subscribe(isHide => _cardViewer.SetView(!isHide))
                .AddTo(this);
        }

        public void SetCardData(TMCardData cardData)
        {
            _cardData = cardData;
            _cardViewer.SetUI(cardData);
        }
    }
}
