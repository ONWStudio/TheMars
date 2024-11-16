using Onw.Attribute;
using Onw.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TM.Buff.Trigger;
using TM.Card;
using TM.Card.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMCardCostModifyBuff : TMDelayBuff, ITMInitializeBuff<TMCardCostModifyBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private List<TMCardModel> _additionalCostCard = new();
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Card_Cost_Modify_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField] public TMCardKindForWhere CardKind { get; private set; }
        [field: SerializeField] public TMCostKind CostKind { get; private set; }
        [field: SerializeField] public int AdditionalCost { get; set; }

        public override LocalizedString Description => throw new System.NotImplementedException();

        public void Initialize(TMCardCostModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            CardKind = creator.CardKind;
            CostKind = creator.CostKind;
            AdditionalCost = creator.AdditionalCost;

            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    CardKind,
                    CostKind,
                    Cost = Mathf.Abs(AdditionalCost),
                    Positive = AdditionalCost >=0
                }
            };
        }

        protected override void OnApplyBuff()
        {
            _additionalCostCard.AddRange(TMCardManager
                .Instance
                .Cards
                .Where(isEqualCard));

            _additionalCostCard.ForEach(card => modifyCost(card, true));

            TMCardManager.Instance.CardCreator.OnPostCreateCard += onPostCreateCard;
        }

        protected override void OnChangedDayByDelayCount(int day)
        {
            TMCardManager.Instance.CardCreator.OnPostCreateCard -= onPostCreateCard;

            _additionalCostCard
                .Where(card => card)
                .ForEach(card => modifyCost(card, false));

            _additionalCostCard.Clear();
            _additionalCostCard = null;
        }

        private void onPostCreateCard(TMCardModel card)
        {
            if (isEqualCard(card)) return;

            modifyCost(card, true);
            _additionalCostCard.Add(card);
        }

        private void modifyCost(TMCardModel card, bool isAdd)
        {
            int additionalCost = isAdd ? AdditionalCost : -AdditionalCost;

            if (CostKind == TMCostKind.CREDIT || CostKind == TMCostKind.ELECTRICITY)
            {
                _additionalCostCard.ForEach(card => card.MainCost.AdditionalCost.Value += additionalCost);
            }
            else
            {
                TMCardData cardData = card.CardData.Value;
                card
                    .SubCosts
                    .Where(isEqualSubCostKind)
                    .ForEach(subCost => subCost.AdditionalCost.Value += additionalCost);
            }
        }

        private bool isEqualSubCostKind(ITMCardSubCostRuntime subCost) => subCost.Cost.CostKind switch
        {
            TMSubCost.MARS_LITHIUM => CostKind == TMCostKind.MARS_LITHIUM,
            TMSubCost.PLANTS => CostKind == TMCostKind.PLANTS,
            TMSubCost.CLAY => CostKind == TMCostKind.CLAY,
            TMSubCost.STEEL => CostKind == TMCostKind.STEEL,
            _ => false
        };

        private bool isEqualCard(TMCardModel card)
        {
            bool isEqual = CardKind switch
            {
                TMCardKindForWhere.EFFECT => card.CardData.Value.Kind == TMCardKind.EFFECT,
                TMCardKindForWhere.CONSTRUCTION => card.CardData.Value.Kind == TMCardKind.CONSTRUCTION,
                _ => true,
            };

            if (isEqual)
            {
                TMCardData cardData = card.CardData.Value;

                isEqual = CostKind switch
                {
                    TMCostKind.MARS_LITHIUM => cardData.CardCosts.Any(cost => cost.CostKind == TMSubCost.MARS_LITHIUM),
                    TMCostKind.CREDIT => cardData.MainCost.CostKind == TMMainCost.CREDIT,
                    TMCostKind.STEEL => cardData.CardCosts.Any(cost => cost.CostKind == TMSubCost.STEEL),
                    TMCostKind.PLANTS => cardData.CardCosts.Any(cost => cost.CostKind == TMSubCost.PLANTS),
                    TMCostKind.CLAY => cardData.CardCosts.Any(cost => cost.CostKind == TMSubCost.CLAY),
                    TMCostKind.ELECTRICITY => cardData.MainCost.CostKind == TMMainCost.ELECTRICITY,
                    _ => false,
                };
            }

            return isEqual;
        }
    }
}