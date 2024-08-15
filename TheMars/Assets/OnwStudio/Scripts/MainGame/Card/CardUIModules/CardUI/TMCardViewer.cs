using System.Collections.Generic;
using Onw.Attribute;
using Onw.Localization;
using TMCard.Effect;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace TMCard.Runtime
{
    // .. View
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        [FormerlySerializedAs("cardImage")]
        [Header("Image")]
        [SerializeField, SelectableSerializeField]
        private Image _cardImage;
        [FormerlySerializedAs("backgroundImage")]
        [SerializeField, SelectableSerializeField]
        private Image _backgroundImage;

        [FormerlySerializedAs("effectField")]
        [Header("Effect Field")]
        [SerializeField, SelectableSerializeField]
        private RectTransform _effectField;

        private bool _isInit;

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

        public void SetUI(TMCardData cardData, IEnumerable<ITMCardEffect> effects)
        {
            _cardImage.sprite = cardData.CardImage;

            foreach (var effect in effects)
            {
                GameObject effectUIObject = new("Effect UI Field");
                effectUIObject.AddComponent<RectTransform>();
                effectUIObject.AddComponent<VerticalLayoutGroup>();
                effectUIObject.transform.SetParent(_effectField.transform, false);

                if (effect is ILocalizable localizable)
                {
                    GameObject labelObject = new("Label Object");
                    labelObject.AddComponent<RectTransform>();
                    labelObject.transform.SetParent(effectUIObject.transform, false);
                    var labelText = labelObject.AddComponent<TextMeshProUGUI>();
                    labelText.alignment = TextAlignmentOptions.Center;
                    labelText.enableAutoSizing = true;
                    labelText.fontSizeMax = 22f;
                    labelText.fontSizeMin = labelText.fontSizeMax * 0.75f;
                    labelText.color = Color.red;

                    if (!localizable.StringOption.TrySetOption(this, label => labelText.text = label, out var localizeStringEvent)) // .. AddComponent
                    {
                        Debug.LogWarning("모노비하이비어가 아닙니다");
                    }
                }
            }
        }
    }
}
