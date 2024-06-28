using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    [SerializeReferenceDropdownName("카드 발견")]
    public sealed class TMCardSelectEffect : ITMCardEffect
    {
        public void OnEffect(TMCardData cardData)
        {
            Debug.Log("카드 발견");
        }
    }
}
