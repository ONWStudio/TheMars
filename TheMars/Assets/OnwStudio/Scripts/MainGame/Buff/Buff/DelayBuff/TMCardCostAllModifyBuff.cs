using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Extensions;
using TM.Buff.Trigger;
using TM.Card.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMCardCostAllModifyBuff : TMDelayBuff, ITMInitializeBuff<TMCardCostAllModifyBuffTrigger>
    {
        [SerializeField, ReadOnly] private List<TMCardModel> _additionalCostCard = new();
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Card_Cost_All_Modify_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;
        [field: SerializeField] public TMCardKindForWhere Kind { get; set; }
        [field: SerializeField] public int AdditionalCost { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMCardCostAllModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            Kind = creator.Kind;
            AdditionalCost = creator.AdditionalCost;
            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    Kind,
                    Cost = Mathf.Abs(AdditionalCost),
                    Positive = AdditionalCost >= 0
                }
            };
        }

        protected override void OnApplyBuff()
        {
            _additionalCostCard.AddRange(TMCardManager
                .Instance
                .Cards
                .Where(isEqualKind));

            _additionalCostCard.ForEach(modifyCost);

            TMCardManager.Instance.CardCreator.OnPostCreateCard += onPostCreateCard;
        }

        protected override void OnChangedDayByDelayCount(int day)
        {
            TMCardManager.Instance.CardCreator.OnPostCreateCard -= onPostCreateCard;

            foreach (TMCardModel card in _additionalCostCard.Where(card => card))
            {
                card.MainCost.AdditionalCost.Value -= AdditionalCost;
                card.SubCosts.ForEach(cost => cost.AdditionalCost.Value -= AdditionalCost);
            }
            
            _additionalCostCard.Clear();
            _additionalCostCard = null;
        }
        
        private bool isEqualKind(TMCardModel card) => Kind switch
        {
            TMCardKindForWhere.EFFECT => card.CardData.Value.Kind == TMCardKind.EFFECT,
            TMCardKindForWhere.CONSTRUCTION => card.CardData.Value.Kind == TMCardKind.CONSTRUCTION,
            _ => true
        };

        private void onPostCreateCard(TMCardModel card)
        {
            if (!isEqualKind(card)) return;
            
            modifyCost(card);
            _additionalCostCard.Add(card);
        }

        private void modifyCost(TMCardModel card)
        {
            card.MainCost.AdditionalCost.Value += AdditionalCost;
            card.SubCosts.ForEach(cost => cost.AdditionalCost.Value += AdditionalCost);
        }
    }
}