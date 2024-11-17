using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Manager;

namespace TM.Usage.Creator
{
    [SerializeField, SerializeReferenceDropdownName("������ ��Ƭ �Ҹ� (n������ ���)")]
    public class TMMarsLithiumUsageCreator : ITMUsageCreator
    {
        [field: SerializeField, DisplayAs("�⺻ ���η�")] public int MarsLithium { get; private set; } = 10;

        public ITMUsage CreateUsage()
        {
            return ITMUsageCreator.CreateEffect<TMMarsLithiumUsage, TMMarsLithiumUsageCreator>(this);
        }
    }
}