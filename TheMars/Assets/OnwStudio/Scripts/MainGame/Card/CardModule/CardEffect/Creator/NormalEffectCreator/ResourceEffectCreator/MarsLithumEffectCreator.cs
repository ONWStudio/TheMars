using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect.Resource
{
    using static ITMEffectCreator;

    [SerializeReferenceDropdownName("�ڿ� ȹ�� (������ ��Ƭ)")]
    public sealed class MarsLithumEffectCreator : IResourceEffectCreator
    {
        [field: SerializeField, DisplayAs("�Ҹ� ��ȭ"), Tooltip("�Ҹ� ��ȭ")] public int Amount { get; private set; }

        public ITMCardEffect CreateEffect()
        {
            return EffectGenerator.CreateEffect<MarsLithumEffect, MarsLithumEffectCreator>(this);
        }
    }
}
