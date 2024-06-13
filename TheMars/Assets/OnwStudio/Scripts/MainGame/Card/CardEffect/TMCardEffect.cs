using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public interface ICardEffect
{
    /// <summary>
    /// .. 카드 사용시
    /// </summary>
    /// <param name="cardObject"></param>
    /// <returns></returns>
    void OnEffect(GameObject cardObject, TMCardData cardData);
}