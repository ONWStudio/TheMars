using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard
{
    using UI;

    /// <summary>
    /// .. 지속 (시간)
    /// </summary>
    [SerializeReferenceDropdownName("지속 (시간)")]
    public sealed class TimeContinuingCard : ICardSpecialEffect
    {
        /// <summary>
        /// .. 지속 시간
        /// </summary>
        [field: SerializeField, DisplayAs("지속 시간")] public float ContinuingTime { get; private set; } = 1f;

        public void ApplyEffect(TMCardController cardController)
        {
        }
    }
}