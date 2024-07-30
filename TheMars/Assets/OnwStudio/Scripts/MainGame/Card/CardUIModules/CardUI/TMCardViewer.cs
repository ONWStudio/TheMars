using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;

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

        public void SetUI(TMCardController controller)
        {
            _cardImage.sprite = controller.CardData.CardImage;

            var args = controller.EffectArgs;

            controller
                .EffectArgs
                .ForEach(effectArg => createEffectUI(_effectField, effectArg.HasLabel, effectArg));

            static void createEffectUI(Transform effectField, bool hasLabel, TMCardEffectArgs effectArg)
            {
                GameObject effectUIObject = new("Effect UI Field");
                effectUIObject.AddComponent<RectTransform>();
                VerticalLayoutGroup verticalLayoutGroup = effectUIObject.AddComponent<VerticalLayoutGroup>();
                effectUIObject.transform.SetParent(effectField.transform, false);

                if (hasLabel)
                {
                    GameObject labelObject = new("Label Object");
                    LocalizeStringEvent localizeStringEvent = labelObject.AddComponent<LocalizeStringEvent>();
                    localizeStringEvent.SetTable("CardSpecialEffectName");
                    labelObject.AddComponent<RectTransform>();
                    labelObject.transform.SetParent(effectUIObject.transform, false);
                    TextMeshProUGUI labelText = labelObject.AddComponent<TextMeshProUGUI>();
                    localizeStringEvent.SetEntry(effectArg.Label);
                    localizeStringEvent.OnUpdateString.AddListener(text => labelText.text = text);
                    localizeStringEvent.RefreshString();
                    labelText.color = Color.red;
                }

                GameObject descriptionObject = new("Description Field");
                descriptionObject.AddComponent<RectTransform>();
                descriptionObject.transform.SetParent(effectUIObject.transform, false);
                TextMeshProUGUI textGUI = descriptionObject.AddComponent<TextMeshProUGUI>();
                textGUI.text = effectArg.Description;
            }
        }
    }
}
