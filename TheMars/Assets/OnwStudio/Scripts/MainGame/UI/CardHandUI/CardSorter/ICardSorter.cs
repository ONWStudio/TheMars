using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 카드들의 정렬하는 형태를 결정하는 추상 클래스 입니다
/// CardHandUI클래스를 참고해주세요
/// </summary>
public interface ICardSorter
{
    /// <summary>
    /// .. 카드들을 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    void SortCards(CardMovementBase[] cardMovementBases, RectTransform rectTransform);
    /// <summary>
    /// .. 특정 인덱스의 카드를 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 카드들을 관리하는 CardHandUI 클래스를 참고해주세요 </param>
    /// <param name="index"> .. 배치시킬 카드의 인덱스 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    void ArrangeCard(CardMovementBase[] cardMovementBases, int index, RectTransform rectTransform);
}
