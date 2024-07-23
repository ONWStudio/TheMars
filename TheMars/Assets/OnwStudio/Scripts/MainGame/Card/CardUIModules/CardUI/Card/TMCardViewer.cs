using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using UniRx;

namespace TMCard.UI
{
    // .. View
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        public Sprite CardSprite
        {
            get => _cardImage.sprite;
            set => _cardImage.sprite = value;
        }

        public Sprite BackgroundSprite
        {
            get => _backgroundImage.sprite;
            set => _backgroundImage.sprite = value;
        }

        [Header("Image")]
        [SerializeField] private Image _cardImage = null;
        [SerializeField, InitializeRequireComponent] private Image _backgroundImage = null;

        [Header("Descriptor")]
        [SerializeField] private TMCardDescriptor _descriptor;

        [SerializeField, InitializeRequireComponent] private Image _raycastingImage = null;

        private bool _isInit = false;

        private void initializeImages()
        {
            _raycastingImage.color = new(255f, 255f, 255f, 0f);
            _cardImage.transform.SetParent(transform, false);
            _cardImage.raycastTarget = false;
            _cardImage.transform.localPosition = Vector3.zero;
        }

        public void Initialize()
        {
            if (_isInit) return;

            _isInit = true;
            initializeImages();
        }

        public void SetDescription(TMCardData cardData)
        {
            _descriptor.SetDescription(cardData);
        }
    }
}
