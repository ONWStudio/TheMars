using System.Collections;
using System.Collections.Generic;
using TMCard.Effect.Resource;
using UnityEngine;
using Onw.Attribute;

namespace TMCard.Effect.Resource
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("�ڿ� ȹ�� (�׶�)")]
    public sealed class TeraResourceEffectCreator : IResourceEffectCreator
    {
        [field: SerializeField, DisplayAs("�Ҹ� ��ȭ"), Tooltip("�Ҹ� ��ȭ")] public int Amount { get; private set; } 

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<TeraResourceEffect, TeraResourceEffectCreator>(this);
        }
    }
}
