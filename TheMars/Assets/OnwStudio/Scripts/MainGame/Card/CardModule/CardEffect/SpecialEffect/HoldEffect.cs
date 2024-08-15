using System.Collections.Generic;
using Onw.Attribute;
using TMCard.Runtime;
using UnityEngine;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 보유
    /// </summary>
    public sealed class HoldEffect : TMCardSpecialEffect, ITMInitializableEffect<HoldEffectCreator>, ITMEffectTrigger
    {
        [SerializeField, DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID"), ReadOnly]
        private TMCardData _friendlyCard;

        private readonly List<ITMNormalEffect> _holdEffects = new();

        public CardEvent OnEffectEvent { get; } = new();

        public void Initialize(HoldEffectCreator effectCreator)
        {
            _friendlyCard = effectCreator.FriendlyCard;
            _holdEffects.AddRange(effectCreator.NormalEffects);
        }

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            TMCardHelper.Instance.OnUsedCard.AddListener(onHoldEffectByFriendlyCard);
            _holdEffects.ForEach(holdEffect => holdEffect?.ApplyEffect(controller, this));

            void onHoldEffectByFriendlyCard(TMCardController usedCard)
            {
                if (!controller)
                {
                    TMCardHelper
                        .Instance
                        .OnUsedCard
                        .RemoveListener(onHoldEffectByFriendlyCard);

                    return;
                }

                if (controller.OnField && _friendlyCard.GetInstanceID() == usedCard.CardData.GetInstanceID())
                {
                    OnEffectEvent.Invoke();
                    Debug.Log("보유 효과 발동");
                }
            }
        }

        public HoldEffect() : base("Hold") {}
    }
}
