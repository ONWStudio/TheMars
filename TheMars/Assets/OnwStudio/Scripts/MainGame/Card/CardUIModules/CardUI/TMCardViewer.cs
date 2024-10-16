using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;
using TMPro;
using Onw.Attribute;
using TM.Card.Effect;

namespace TM.Card.Runtime
{
    /// <summary>
    /// .. 눈에 보이는 정보(카드 설명, 효과, 이름, 코스트, 종류) 등에 관해 관리하는 Viewer클래스 입니다 CardData의 정보대로 최종적으로 카드를 출력하게 됩니다
    /// </summary>
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

        [SerializeField, InitializeRequireComponent]
        private Image _raycastImage;
        
        [Header("Description Option")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _descriptionText;

        [Header("Name Option")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _nameText;

        [Header("Cost Option")]
        [SerializeField, SelectableSerializeField]
        private Image _costFieldImage;
        
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _costText;
        
        private Action<Locale> _onLanguageChanged = null;
        
        private void buildDescriptionText(ITMCardEffect[] effects)
        {
            _descriptionText.text =
                string.Join("\n", effects.OfType<ITMNormalEffect>().Select(effect => effect.Description));
        }

        public void SetView(bool isOn)
        {
            _cardImage.enabled = isOn;
            _backgroundImage.enabled = isOn;
            _descriptionText.enabled = isOn;
            _nameText.enabled = isOn;
            _costFieldImage.enabled = isOn;
            _costText.enabled = isOn;
        }
        
        public void SetUI(TMCardData cardData)
        {
            _cardImage.sprite = cardData.CardImage;

            // buildDescriptionText(effects);
            // _onLanguageChanged = locale => buildDescriptionText(effects);

            // foreach (INotifier notifier in effects.OfType<INotifier>())
            // {
            //     notifier.Event.AddListener(eventType => buildDescriptionText(effects));
            // }
            
            _nameText.text = cardData.CardName;
            _onLanguageChanged += locale => _nameText.text = cardData.CardName;
            
            LocalizationSettings.SelectedLocaleChanged += _onLanguageChanged;
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= _onLanguageChanged;
        }
    }
}