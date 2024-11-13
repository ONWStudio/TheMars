using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Extensions;
using TM.Buff.Trigger;
using TM.Card.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TM.Buff
{
    [System.Serializable]
    public class TMCardCostAddBuff : TMDelayBuff, ITMInitializeBuff<TMCardCostAddBuffTrigger>
    {
        [field: SerializeField] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField] public TMCardKindForWhere Kind { get; set; }
        [field: SerializeField] public int AdditionalCost { get; set; }

        [SerializeField, ReadOnly] private List<TMCardModel> _additionalCostCard = new();

        public void Initialize(TMCardCostAddBuffTrigger creator)
        {
            base.Initialize(creator);

            Kind = creator.Kind;
            AdditionalCost = creator.AdditionalCost;
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
            TMCardKindForWhere.EFFECT => card.CardData.Kind == TMCardKind.EFFECT,
            TMCardKindForWhere.CONSTRUCTION => card.CardData.Kind == TMCardKind.CONSTRUCTION,
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