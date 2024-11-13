using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Event.Effect.Creator
{
    [System.Serializable]
    public abstract class TMEventResourceAddEffectBaseCreator : ITMEventEffectCreator
    {
        [field: SerializeField, DisplayAs("�ڿ� ����")] public TMResourceKind ResourceKind { get; private set; } = TMResourceKind.CREDIT;

        public abstract ITMEventEffect CreateEffect();
    }
}
