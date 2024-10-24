using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using TM.Card.Effect;
using TM.Card.Effect.Creator;
using TM.Card.Runtime;
using UnityEngine;

namespace TM.Event
{
    public sealed class TMGovernanceSystemChangeEventData : TMEventData
    {
        [field: Header("시민들과 따로 얘기 할 장소를 만들자 선택지")]
        [field: SerializeField, Min(0), DisplayAs("소모 강철")] public int TopSteelSubtract { get; private set; } = 20;
        [field: SerializeField, Min(0), DisplayAs("소모 크레딧")] public int TopCreditSubtract { get; private set; } = 30;
        [field: SerializeField, Min(0), DisplayAs("획득 만족도")] public int TopSatisfactionAdd { get; private set; } = 20;

        [field: Header("기업과 얘기 해봐야겠군 선택지")]
        [field: SerializeField, Min(0), DisplayAs("획득 크레딧")] public int BottomCreditAdd { get; private set; } = 40;
        [field: SerializeField, Min(0), DisplayAs("획득 만족도")] public int BottomSatisfactionAdd { get; private set; } = 10;

        // .. 시민들과 따로 얘기 할 장소를 만들자
        public override bool CanFireTop => TMPlayerManager.Instance.Steel >= TopSteelSubtract &&
                                           TMPlayerManager.Instance.Credit >= TopCreditSubtract && 
                                           TMCardManager.Instance.Cards.Any(card => card.CardEffect is TMCardBuildingCreateEffect);
        public override bool CanFireBottom => true;
        
        public override List<object> TopEffectLocalizedArguments 
            => new() { TopSteelSubtract, TopCreditSubtract, TopSatisfactionAdd };
        
        public override List<object> BottomEffectLocalizedArguments
            => new() { BottomCreditAdd, BottomSatisfactionAdd };

        protected override void TriggerLeftEvent()
        {
            TMCardModel card = TMCardManager
                .Instance
                .Cards
                .FirstOrDefault(card => card.CardEffect is TMCardBuildingCreateEffect);

            if (card)
            {
                TMCardManager.Instance.RemoveCard(card);
                Destroy(card.gameObject);

                TMPlayerManager.Instance.Steel -= TopSteelSubtract;
                TMPlayerManager.Instance.Credit -= TopCreditSubtract;
                TMPlayerManager.Instance.Satisfaction += TopSatisfactionAdd;
            }
        }
        
        protected override void TriggerRightEvent()
        {
            TMPlayerManager.Instance.Credit += BottomCreditAdd;
            TMPlayerManager.Instance.Satisfaction += BottomSatisfactionAdd;
            TMCardModel card = TMCardManager
                .Instance
                .CardCreator
                .CreateCardByCondition(cardData => cardData.CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>());
            TMCardManager.Instance.AddCard(card);
        }
    }
}
