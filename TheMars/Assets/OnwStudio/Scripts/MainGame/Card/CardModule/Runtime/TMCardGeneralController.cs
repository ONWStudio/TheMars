using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Onw.Attribute;
using Onw.Extensions;
using UnityEditor;

namespace TMCard.Runtime
{
    // .. Controller
    [DisallowMultipleComponent]
    public sealed class TMCardGeneralController : MonoBehaviour
    {
        [Header("Model")]
        [SerializeField, InitializeRequireComponent] private TMCardController _cardController;

        [Header("View")]
        [SerializeField, InitializeRequireComponent] private TMCardViewer _cardViewer;

        private TMCardData _cardData = null;

        private void Start()
        {
            if (_cardController.CardData)
            {
                SetCardData(_cardController.CardData);
            }
            else
            {
                this
                    .ObserveEveryValueChanged(controller => _cardController.CardData)
                    .Take(1)
                    .Subscribe(SetCardData)
                    .AddTo(this);
            }
        }

        public void SetCardData(TMCardData cardData)
        {
            _cardData = cardData;
            _cardViewer.SetUI(_cardController);
        }
    }
}
