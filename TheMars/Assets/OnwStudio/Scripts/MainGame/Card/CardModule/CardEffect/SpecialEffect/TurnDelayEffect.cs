using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 소요 (턴)
    /// </summary>
    public sealed class TurnDelayEffect : TMCardSpecialEffect, ITMInitializableEffect<TurnDelayEffectCreator>
    {
        /// <summary>
        /// .. 소요시킬 턴
        /// </summary>
        [field: SerializeField, DisplayAs("소요 턴"), ReadOnly] public int DelayTurn { get; private set; } = 5;

        public void Initialize(TurnDelayEffectCreator effectCreator)
        {
            DelayTurn = effectCreator.DelayTurn;
        }

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => TMCardHelper.Instance.DelayTurn(controller, DelayTurn));
        }

        public TurnDelayEffect() : base("TurnDelay") {}
    }
}
