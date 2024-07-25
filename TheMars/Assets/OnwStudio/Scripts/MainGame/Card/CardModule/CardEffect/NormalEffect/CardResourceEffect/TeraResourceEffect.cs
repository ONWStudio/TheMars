using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect.Resource
{
    [SerializeReferenceDropdownName("자원 획득 (테라)")]
    public sealed class TeraResourceEffect : TMCardResourceEffect
    {
        public override void OnResourceEffect(TMCardController controller, int addtionalAmount)
        {
            Debug.Log(Amount + addtionalAmount);
            Debug.Log("테라 획득");
        }
    }
}