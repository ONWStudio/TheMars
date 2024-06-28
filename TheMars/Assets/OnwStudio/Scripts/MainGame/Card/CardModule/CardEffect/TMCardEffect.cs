using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    public interface ITMCardEffect
    {
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        /// <returns></returns>
        void OnEffect(TMCardData cardData);
    }
}