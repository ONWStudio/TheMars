using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Localization;
using TMPro;
using TMCard.Effect;

namespace TMCard.Runtime
{
    // .. View
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _cardImage = null;
        [SerializeField, SelectableSerializeField] private Image _backgroundImage = null;

        [Header("Effect Field")]
        [SerializeField, SelectableSerializeField] private RectTransform _effectField = null;

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

        public void SetUI(TMCardData cardData, IEnumerable<ITMCardEffect> effects)
        {
            _cardImage.sprite = cardData.CardImage;

            foreach (ITMCardEffect effect in effects)
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
                    TextMeshProUGUI labelText = labelObject.AddComponent<TextMeshProUGUI>();
                    labelText.alignment = TextAlignmentOptions.Center;
                    labelText.enableAutoSizing = true;
                    labelText.fontSizeMax = 22f;
                    labelText.fontSizeMin = labelText.fontSizeMax * 0.75f;
                    labelText.color = Color.red;

                    if (!localizable.StringOption.TrySetOption(this, label => labelText.text = label, out LocalizeStringEvent localizeStringEvent)) // .. AddComponent
                    {
                        Debug.LogWarning("모노비하이비어가 아닙니다");
                    }
                }
            }
        }
    }
}
