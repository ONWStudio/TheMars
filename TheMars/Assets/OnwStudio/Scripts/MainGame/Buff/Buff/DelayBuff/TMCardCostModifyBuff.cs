using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using Onw.Extensions;
using TM.Card;
using TM.Card.Runtime;
using TM.Buff.Trigger;

namespace TM.Buff
{
    [System.Serializable]
    public class TMCardCostModifyBuff : TMDelayBuff, ITMInitializeBuff<TMCardCostModifyBuffTrigger>
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private List<TMCardModel> _additionalCostCard = new();
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Card_Cost_Modify_Buff");

        public override Color IconBackgroundColor => AdditionalCost >= 0 ?
            ColorUtility.TryParseHtmlString("#2C138E", out Color blue) ? blue : Color.blue :
            ColorUtility.TryParseHtmlString("#8E3214", out Color red) ? red : Color.red;

        public override AssetReferenceSprite IconReference => _iconReference;

        [field: SerializeField] public TMCardKindForWhere CardKind { get; private set; }
        [field: SerializeField] public TMResourceKind CostKind { get; private set; }
        [field: SerializeField] public int AdditionalCost { get; set; }

        public override LocalizedString Description => throw new System.NotImplementedException();

        public void Initialize(TMCardCostModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            CardKind = creator.CardKind;
            CostKind = creator.CostKind;
            AdditionalCost = creator.AdditionalCost;
            bool positive = AdditionalCost >= 0;

            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    CardKind,
                    CostKind,
                    Cost = Mathf.Abs(AdditionalCost),
                    Positive = positive
                }
            };

            _iconReference = new(positive ? "CardCost-plus" : "Cardcost-minus");
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
            
            if (card.MainCost.Kind == CostKind)
            {
                card.MainCost.AdditionalCost.Value += additionalCost;
            }
            else
            {
                card
                    .SubCosts
                    .Where(subCost => subCost.Kind == CostKind)
                    .ForEach(subCost => subCost.AdditionalCost.Value += additionalCost);
            }
        }

        private bool isEqualCard(TMCardModel card)
        {
            bool isEqual = CardKind switch
            {
                TMCardKindForWhere.EFFECT => card.CardData.Value.Kind == TMCardKind.EFFECT,
                TMCardKindForWhere.CONSTRUCTION => card.CardData.Value.Kind == TMCardKind.CONSTRUCTION,
                _ => true,
            };

            return isEqual;
        }
    }
}