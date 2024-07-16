using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMCard.UI
{
    public sealed class TMCardUIController : MonoBehaviour
    {
        public Sprite CardSprite
        {
            get => _cardImage.sprite;
            set => _cardImage.sprite = value;
        }

        [Header("Image")]
        [SerializeField] private Image _cardImage = null;

        [Header("Descriptor")]
        [SerializeField] private TMCardDescriptor _descriptor;

        private Image _raycastingImage = null;

        private void Awake()
        {
            _raycastingImage = gameObject.GetComponent<Image>();
        }

        private void Start()
        {
            initializeImages();
        }

        private void initializeImages()
        {
            _raycastingImage.color = new(255f, 255f, 255f, 0f);
            _cardImage.transform.SetParent(transform, false);
            _cardImage.raycastTarget = false;
            _cardImage.transform.localPosition = Vector3.zero;
        }

        public void SetDescription(TMCardData cardData)
        {
            _descriptor.SetDescription(cardData);
        }
    }
}
