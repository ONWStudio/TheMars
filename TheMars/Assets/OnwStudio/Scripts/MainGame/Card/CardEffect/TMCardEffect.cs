using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public sealed partial class TMCardData : ScriptableObject
{
    public interface ICardEffect
    {
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        /// <param name="cardObject"></param>
        /// <returns></returns>
        MMF_Feedback[] OnEffectStart(GameObject cardObject, TMCardData cardData);
        MMF_Feedback[] OnEffectEnded(GameObject cardObject, TMCardData cardData);
    }
}
