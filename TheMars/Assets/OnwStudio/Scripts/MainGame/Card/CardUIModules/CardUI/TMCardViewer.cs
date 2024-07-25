using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;

namespace TMCard.Runtime
{
    // .. View
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _cardNameText = null;

        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _cardImage = null;
        [SerializeField, InitializeRequireComponent] private Image _backgroundImage = null;

        //[Header("Descriptor")]
        //[SerializeField, SelectableSerializeField] private TMCardDescriptor _descriptor;

        private bool _isInit = false;

        private void initializeImages()
        {
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

        public void SetUI(TMCardData cardData)
        {
        }
    }
}
