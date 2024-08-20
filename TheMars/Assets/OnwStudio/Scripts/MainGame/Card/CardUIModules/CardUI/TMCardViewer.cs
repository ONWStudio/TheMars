using System.Collections.Generic;
using Onw.Attribute;
using Onw.Localization;
using TMCard.Effect;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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

        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _nameText;

        private bool _isInit;

        public void SetUI(TMCardData cardData, IEnumerable<ITMCardEffect> effects)
        {
            _cardImage.sprite = cardData.CardImage;

            #region EffectSet
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

                    if (!localizable.StringOption.TrySetOption(this, label => labelText.text = label, out LocalizeStringEvent _)) // .. AddComponent
                    {
                        Debug.LogWarning("라벨이 없거나 모노비하이비어가 null입니다");
                    }
                }
            }
            #endregion
            #region NameSet
            if (!cardData.CardName.TrySetOption(this, cardName => _nameText.text = cardName, out LocalizeStringEvent _))
            {
                Debug.LogWarning("카드 이름이 존재하지 않습니다");
            }
            #endregion
        }
    }
}
