using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 보유
    /// </summary>
    [SerializeReferenceDropdownName("보유"), Substitution("보유")]
    public sealed partial class HoldEffect : ITMCardSpecialEffect
    {
        [SerializeField, DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID")]
        private TMCardData _friendlyCard = null;

        [SerializeReference, DisplayAs("보유 효과"), Tooltip("보유 효과"), SerializeReferenceDropdown]
        private List<ITMNormalEffect> _holdEffects = new();

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("보유");

        public void ApplyEffect(TMCardController cardController)
        {
            TMCardGameManager.Instance.OnUsedCard.AddListener(onHoldEffectByFriendlyCard);

            // .. HoldCard카드는 게임상에서 스크립터블 오브젝트를 통해 참조하므로 클래스 멤버 함수로 선언하면 cardController가 마지막에 ApplyEffect를 호출했던 카드 컨트롤러로 고정된다
            // 그러므로 로컬함수로 ApplyEffect를 호출할때마다 onHoldEffectByFriendlyCard 인스턴스를 생성해야한다
            // .. 로컬함수는 Action<TMCardController> onHoldEffectByFriendlyCard와 같다 딜리게이트 인스턴스를 새로 생성하는 것과 같으므로 ..
            void onHoldEffectByFriendlyCard(TMCardController usedCard)
            {
                if (!cardController)
                {
                    TMCardGameManager
                        .Instance
                        .OnUsedCard
                        .RemoveListener(onHoldEffectByFriendlyCard);

                    return;
                }

                if (cardController.OnField && _friendlyCard.Guid == usedCard.CardData.Guid)
                {
                    _holdEffects
                        .ForEach(holdEffect => holdEffect?.ApplyEffect(cardController));

                    Debug.Log("보유 효과 발동");
                }
            }
        }
    }
}
