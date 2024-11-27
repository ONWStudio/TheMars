using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Prototype;
using Onw.UI.Components;
using TM.Cost;

namespace TM.Card.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardViewer : MonoBehaviour
    {
        [FormerlySerializedAs("cardImage")]
        [Header("Image")]
        [SerializeField, SelectableSerializeField]
        private Image _cardImage;

        [SerializeField, SelectableSerializeField]
        private TMCardCostIcon _mainCostViewer;

        [SerializeField, SelectableSerializeField]
        private Mask _mask;

        [SerializeField, SelectableSerializeField]
        private RectTransform _subCostField;

        [SerializeField]
        private AssetReferenceGameObject _costIconReference;
        
        public void SetView(bool isOn)
        {
            _mask.enabled = !isOn;
        }
        
        public void SetUI(TMCardModel cardModel)
        {
            _cardImage.sprite = cardModel.CardData.Value.CardImage;

            setCost(_mainCostViewer, cardModel.MainCost);

            cardModel.SubCosts.ForEach(cost => PrototypeManager.Instance.ClonePrototypeFromReferenceAsync<TMCardCostIcon>(_costIconReference, icon => onLoadedCostIcon(icon, cost)));

            void onLoadedCostIcon(TMCardCostIcon costViewer, ITMResourceCost cost)
            {
                costViewer.SetParent(_subCostField, false);
                setCost(costViewer, cost);
            }

            void setCost(TMCardCostIcon costViewer, ITMResourceCost cost)
            {
                costViewer.SetIcon(cost.Kind);
                costViewer.SetCost(cost.FinalCost);
            }
        }
    }
}