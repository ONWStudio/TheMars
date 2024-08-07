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

        //[Header("Descriptor")]
        //[SerializeField, SelectableSerializeField] private TMCardDescriptor _descriptor;
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

            effects.ForEach(effectArg => createEffectUI(this, _effectField, effectArg));

            static void createEffectUI(MonoBehaviour mono, Transform effectField, ITMCardEffect effect)
            {
                GameObject effectUIObject = new("Effect UI Field");
                effectUIObject.AddComponent<RectTransform>();
                VerticalLayoutGroup verticalLayoutGroup = effectUIObject.AddComponent<VerticalLayoutGroup>();
                effectUIObject.transform.SetParent(effectField.transform, false);

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

                    if (!localizable.StringOption.TrySetOption(mono, labelText, out LocalizeStringEvent localizeStringEvent))
                    {
                        Debug.LogWarning("UI가 초기화 되지 않았습니다");
                    }
                }

                //if ()
                //GameObject descriptionObject = new("Description Field");
                //descriptionObject.AddComponent<RectTransform>();
                //descriptionObject.transform.SetParent(effectUIObject.transform, false);
                //TextMeshProUGUI textGUI = descriptionObject.AddComponent<TextMeshProUGUI>();
                //textGUI.text = effectArg.Description;
            }
        }
    }
}
