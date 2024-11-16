using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using Onw.Attribute;
using Onw.Extensions;
using UnityEngine.ResourceManagement.AsyncOperations;
using Onw.Manager.Prototype;

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

        [SerializeField, InitializeRequireComponent]
        private Mask _mask;

        private Action<Locale> _onLanguageChanged = null;

        [SerializeField, SelectableSerializeField] private TMCardCostIcon _mainCostIcon;
        [SerializeField, SelectableSerializeField] private RectTransform _subCostField;

        AsyncOperationHandle<Sprite> _costIconHandle;

        public void SetView(bool isOn)
        {
            _mask.enabled = !isOn;
        }

        public void SetUI(TMCardModel cardModel)
        {
            TMCardData cardData = cardModel.CardData.Value;
            _cardImage.sprite = cardData.CardImage;

            TMResourceKind kind = cardData.MainCost.CostKind switch
            {
                TMMainCost.ELECTRICITY => TMResourceKind.ELECTRICITY,
                _ => TMResourceKind.CREDIT,
            };

            _mainCostIcon.SetIcon(kind);
            cardModel.MainCost.AdditionalCost.AddListener(onChangedAdditionalCostByMain);

            for (int i = 0; i < cardData.CardCosts.Count; i++)
            {
                TMCardSubCost subCost = cardData.CardCosts[i];
                TMResourceKind subCostkind = subCost.CostKind switch
                {
                    TMSubCost.STEEL => TMResourceKind.STEEL,
                    TMSubCost.PLANTS => TMResourceKind.PLANTS,
                    TMSubCost.CLAY => TMResourceKind.CLAY,
                    _ => TMResourceKind.MARS_LITHIUM,
                };

                ITMCardCostRuntime subCostRuntime = cardModel.SubCosts[i];
                PrototypeManager.Instance.ClonePrototypeAsync<TMCardCostIcon>("CostUI", onLoadedCostIcon, _subCostField);

                void onLoadedCostIcon(TMCardCostIcon icon)
                {
                    icon.SetIcon(subCostkind);
                    subCostRuntime.AdditionalCost.AddListener(onChangedAddtionalCostBySub);

                    void onChangedAddtionalCostBySub(int cost)
                    {
                        icon.SetCost(subCostRuntime.FinalCost);
                    }
                }
            }

            void onChangedAdditionalCostByMain(int cost)
            {
                _mainCostIcon.SetCost(cardModel.MainCost.FinalCost);
            }
        }
    }
}