//using System.Buffers;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Onw.Attribute;
//using TM.Buff;
//using TM.Card.Effect;
//using TM.Card.Effect.Creator;
//using TM.Card.Runtime;
//using TM.Manager;
//using TM.Runtime;
//using UnityEngine;

//namespace TM.Event
//{
//    [Substitution("거버넌스 체제 변화")]
//    public sealed class TMGovernanceSystemChangeEventData : TMEventData
//    {
//        [field: Header("시민들과 따로 얘기 할 장소를 만들자 선택지")]
//        [field: SerializeField, DisplayAs("소모 강철"), OnwMin(0)] public int TopSteelSubtract { get; private set; } = 20;
//        [field: SerializeField, DisplayAs("소모 크레딧"), OnwMin(0)] public int TopCreditSubtract { get; private set; } = 30;
//        [field: SerializeField, DisplayAs("만족도 증가 설정")] public TMResourceRepeatAddBuffTrigger ResourceRepeatAddBuffTrigger { get; private set; } = new();

//        [field: Header("기업과 얘기 해봐야겠군 선택지")]
//        [field: SerializeField, DisplayAs("획득 크레딧"), OnwMin(0)] public int BottomCreditAdd { get; private set; } = 40;
//        [field: SerializeField, DisplayAs("획득 만족도"), OnwMin(0)] public int BottomSatisfactionAdd { get; private set; } = 10;

//         .. 시민들과 따로 얘기 할 장소를 만들자
//        public override bool CanFireTop => TMPlayerManager.Instance.Steel.Value >= TopSteelSubtract &&
//                                           TMPlayerManager.Instance.Credit.Value >= TopCreditSubtract;
//        public override bool CanFireBottom => true;

//        public override Dictionary<string, object> TopEffectLocalizedArguments => new()
//        {
//            { "SteelSubtract", TopSteelSubtract },
//            { "CreditSubtract", TopCreditSubtract },
//        };

//        public override Dictionary<string, object> BottomEffectLocalizedArguments => new()
//        {
//            { "CreditAdd", BottomCreditAdd },
//            { "SatisfactionAdd", BottomSatisfactionAdd }
//        };

//        protected override void TriggerTopEvent()
//        {
//            TMCardModel card = TMCardManager
//                .Instance
//                .Cards
//                .FirstOrDefault(card => card.CardEffect is TMCardBuildingCreateEffect);

//            TMPlayerManager.Instance.Steel.Value -= TopSteelSubtract;
//            TMPlayerManager.Instance.Credit.Value -= TopCreditSubtract;
//            ResourceRepeatAddBuffTrigger.CreateBuff().ApplyBuff();


//            if (card)
//            {
//                TMCardManager.Instance.RemoveCard(card);
//                Destroy(card.gameObject);
//            }
//        }

//        protected override void TriggerBottomEvent()
//        {
//            TMPlayerManager.Instance.Credit.Value += BottomCreditAdd;
//            TMPlayerManager.Instance.Satisfaction.Value += BottomSatisfactionAdd;
//            TMCardModel[] cardArray = new TMCardModel[1];

//            cardArray[0] = TMCardManager
//                .Instance
//                .CardCreator
//                .CreateRandomCardByWhere(cardData => cardData.CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>());

//            TMCardNotifyIconSpawner.Instance.CreateIcon(cardArray);
//        }
//    }
//}