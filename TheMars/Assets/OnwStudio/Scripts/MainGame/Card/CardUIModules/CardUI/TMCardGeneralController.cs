using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Onw.Attribute;
using Onw.Extensions;
using UnityEditor;

namespace TMCard.UI
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
        private readonly CompositeDisposable _disposables = new();

        private void initializeReactiveProperties()
        {
            observe(cardData => cardData.CardName, Action<T> );
            observe(cardData => cardData.Description, _description);
            observe(cardData => cardData.CardImage, _cardImage);
            observe(cardData => cardData.Resource, _resource);

            void observe<T>(Func<TMCardData, T> selector, Action<T> action)
            {
                if (!_cardData) return;

                this
                    .ObserveEveryValueChanged(_ => selector.Invoke(_cardData))
                    .Subscribe(action)
                    .AddTo(_disposables);
            }
        }

        public void SetCardData(TMCardData cardData)
        {
            if (Application.isPlaying && _cardData) return;

            _cardData = cardData;
            initializeReactiveProperties();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
