using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCard.Runtime;

namespace TMCard.Effect.Resource
{
    [SerializeReferenceDropdownName("자원 획득 (마르스 리튬)")]
    public sealed class MarsLithumEffect : TMCardResourceEffect
    {
        public override void OnResourceEffect(TMCardController controller, int addtionalAmount)
        {
            Debug.Log(Amount + addtionalAmount);
            Debug.Log("마르스 리튬 획득");
        }
    }
}

