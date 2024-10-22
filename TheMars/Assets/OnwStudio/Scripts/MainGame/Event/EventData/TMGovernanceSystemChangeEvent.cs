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
    public sealed class TMGovernanceSystemChangeEvent : TMEventData
    {
        [field: Header("시민들과 따로 얘기 할 장소를 만들자 선택지")]
        [field: SerializeField, Range(0, 50), DisplayAs("소모 강철")] public int LeftSteelSubtract { get; private set; } = 20;
        [field: SerializeField, Range(0, 50), DisplayAs("소모 크레딧")] public int LeftCreditSubtract { get; private set; } = 30;
        [field: SerializeField, Range(10, 40), DisplayAs("획득 만족도")] public int LeftSatisfactionAdd { get; private set; } = 20;

        [field: Header("기업과 얘기 해봐야겠군 선택지")]
        [field: SerializeField, Range(10, 60), DisplayAs("획득 크레딧")] public int RightCreditAdd { get; private set; } = 40;
        [field: SerializeField, Range(10, 30), DisplayAs("획득 만족도")] public int RightSatisfactionAdd { get; private set; } = 10;

        public override string Description { get; }
        
        public override string LeftDescription
        {
            get;
        }
        
        public override string RightDescription
        {
            get;
        }
        
        public override string HeaderDescription
        {
            get;
        }
        
        // .. 시민들과 따로 얘기 할 장소를 만들자
        protected override void TriggerLeftEvent()
        {
            if (TMPlayerManager.Instance.Steel < LeftSteelSubtract || 
                TMPlayerManager.Instance.Credit < LeftCreditSubtract) return;

            TMCardModel card = TMCardManager
                .Instance
                .Cards
                .FirstOrDefault(card => card.CardEffect is TMCardBuildingCreateEffect);

            if (card)
            {
                TMCardManager.Instance.RemoveCard(card);
                Destroy(card.gameObject);

                TMPlayerManager.Instance.Steel -= LeftSteelSubtract;
                TMPlayerManager.Instance.Credit -= LeftCreditSubtract;
                TMPlayerManager.Instance.Satisfaction += LeftSatisfactionAdd;
            }
        }
        
        protected override void TriggerRightEvent()
        {
            TMPlayerManager.Instance.Credit += RightCreditAdd;
            TMPlayerManager.Instance.Satisfaction += RightSatisfactionAdd;
            TMCardModel card = TMCardManager
                .Instance
                .CardCreator
                .CreateCardByCondition(cardData => cardData.CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>());
            TMCardManager.Instance.AddCard(card);
        }
    }
}
