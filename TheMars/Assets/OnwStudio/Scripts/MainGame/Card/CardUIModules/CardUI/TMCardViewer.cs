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
            _cardImage.sprite = cardModel.CardData.Value.CardImage;
            _costText.text = cardModel.MainCost.FinalCost.ToString();
            
            string reference = cardModel.MainCost.Kind switch
            {
                TMResourceKind.CREDIT => "Credit_Icon",
                TMResourceKind.ELECTRICITY => "Electricity_Icon",
                TMResourceKind.MARS_LITHIUM => "MarsLithium_Icon",
                TMResourceKind.POPULATION => "Population_Icon",
                TMResourceKind.STEEL => "Steel_Icon",
                TMResourceKind.PLANT => "Plant_Icon",
                TMResourceKind.CLAY => "Clay_Icon",
                TMResourceKind.SATISFACTION => "Satisfaction_Icon",
                _ => null
            };

            if (reference is not null)
            {
                _spriteHandle = Addressables.LoadAssetAsync<Sprite>(reference);
                _spriteHandle.Completed += onCompletedSprite;

                void onCompletedSprite(AsyncOperationHandle<Sprite> spriteHandle)
                {
                    _costImage.sprite = spriteHandle.Result;
                }
            }
        }

        private void OnDestroy()
        {
            if (!_spriteHandle.IsValid()) return;
            
            Addressables.Release(_spriteHandle);
        }
    }
}