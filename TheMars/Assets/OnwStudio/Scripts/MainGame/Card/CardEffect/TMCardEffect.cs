using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffect
{
    /// <summary>
    /// .. 카드 사용시
    /// </summary>
    /// <param name="cardObject"></param>
    /// <returns></returns>
    void OnEffect(GameObject cardObject, TMCardData cardData);
}

/// <summary>
/// .. 지속 효과가 적용이 가능한 카드의 경우 해당 인터페이스 적용
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICardOffEffect<T> where T : ICardEffect
{
    void OffEffect(GameObject cardObject, TMCardData cardData);
}