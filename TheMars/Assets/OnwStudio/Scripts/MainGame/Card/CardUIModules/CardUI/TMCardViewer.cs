using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using Onw.Attribute;

namespace TM.Card.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        [FormerlySerializedAs("cardImage")]
        [Header("Image")]
        [SerializeField, SelectableSerializeField]
        private Image _cardImage;

        [Header("Cost Option")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _costText;

        [SerializeField, SelectableSerializeField]
        private Image _costImage;

        [SerializeField, SelectableSerializeField]
        private Mask _mask = null;
        
        private Action<Locale> _onLanguageChanged = null;

        private AsyncOperationHandle<Sprite> _spriteHandle;

        public void SetView(bool isOn)
        {
            _mask.enabled = !isOn;
        }
        
        public void SetUI(TMCardModel cardModel)
        {
            TMCardData cardData = cardModel.CardData.Value;
            _cardImage.sprite = cardData.CardImage;
            _costText.text = cardData.MainCost.Cost.ToString();
            
            string reference = cardData.MainCost.CostKind switch
            {
                TMMainCost.CREDIT => "Credit_Icon",
                TMMainCost.ELECTRICITY => "Electricity_Icon",
                _ => null
            };

            _spriteHandle = Addressables.LoadAssetAsync<Sprite>(reference);
            _spriteHandle.Completed += onCompletedSprite;

            void onCompletedSprite(AsyncOperationHandle<Sprite> spriteHandle)
            {
                _costImage.sprite = spriteHandle.Result;
            }
        }

        private void OnDestroy()
        {
            if (!_spriteHandle.IsValid()) return;
            
            Addressables.Release(_spriteHandle);
        }
    }
}