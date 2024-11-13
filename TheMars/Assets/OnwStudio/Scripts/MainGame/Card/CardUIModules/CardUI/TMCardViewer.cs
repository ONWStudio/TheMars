using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Interface;
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
        
        [Header("Cost Option")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _costText;

        [SerializeField, SelectableSerializeField]
        private Image _costImage;
        
        private Action<Locale> _onLanguageChanged = null;

        [Header("Resource")]
        [SerializeField]
        private Sprite _electricitySprite;

        [SerializeField]
        private Sprite _creditSprite;

        [FormerlySerializedAs("_viewerComponents")]
        [SerializeField] private Behaviour[] _viewComponents;
        
        private void buildDescriptionText(ITMCardEffect[] effects)
        {
            //_descriptionText.text =
            //    string.Join("\n", effects.OfType<ITMNormalEffect>().Select(effect => effect.Description));
        }

        public void SetView(bool isOn)
        {
            _viewComponents.ForEach(component => component.enabled = isOn);
        }
        
        public void SetUI(TMCardModel cardModel)
        {
            TMCardData cardData = cardModel.CardData.Value;
            _cardImage.sprite = cardData.CardImage;
            _costText.text = cardData.MainCost.Cost.ToString();
            _costImage.sprite = cardData.MainCost.CostKind switch
            {
                TMMainCost.CREDIT => _creditSprite,
                TMMainCost.ELECTRICITY => _electricitySprite,
                _ => null
            };
        }
    }
}