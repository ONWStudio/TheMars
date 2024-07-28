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
    public sealed class TurnDelayEffect : ITMCardSpecialEffect, ITMInitializableEffect<TurnDelayEffectCreator>
    {
        /// <summary>
        /// .. 소요시킬 턴
        /// </summary>
        [field: SerializeField, DisplayAs("소요 턴"), ReadOnly] public int DelayTurn { get; private set; } = 5;

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("소요(턴)");

        public void ApplyEffect(TMCardController cardController)
        {
            cardController.UseState = ()
                => TMCardGameManager.Instance.DelayTurn(cardController, DelayTurn);
        }

        public void Initialize(TurnDelayEffectCreator effectCreator)
        {
            DelayTurn = effectCreator.DelayTurn;
        }
    }
}
