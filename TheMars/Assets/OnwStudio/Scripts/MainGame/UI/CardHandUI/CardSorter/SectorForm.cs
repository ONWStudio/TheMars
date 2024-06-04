using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UITools.UITool;

/// <summary>
/// .. 카드를 부채꼴의 형태로 정렬시키는 클래스 입니다
/// 높이는 너비 기준 비율로 정합니다
/// </summary>
public sealed class SectorForm : ICardSorter
{
    /// <summary>
    /// .. 최대 앵글 값
    /// </summary>    
    public float MaxAngle { get; init; } = 128f;
    /// <summary>
    /// .. 높이 비율
    /// </summary>
    public float HeightRatioFromWidth { get; init; } = 0.25f;

    /// <summary>
    /// .. 특정 인덱스의 카드를 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="index"> .. 배치시킬 카드의 인덱스 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    public void ArrangeCard(CardMovementBase[] cardMovementBases, int index, RectTransform rectTransform)
    {
        if (cardMovementBases.Length <= 0 || cardMovementBases.Length <= index) return;

        float angleStep = MaxAngle / (cardMovementBases.Length - 1);
        float startAngle = -MaxAngle * 0.5f;

        runTargetTransform(cardMovementBases[index], index, angleStep, startAngle, rectTransform);
    }

    /// <summary>
    /// .. 카드들을 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    public void SortCards(CardMovementBase[] cardMovementBases, RectTransform rectTransform)
    {
        float angleStep = MaxAngle / (cardMovementBases.Length - 1);
        float startAngle = -MaxAngle * 0.5f;

        for (int i = 0; i < cardMovementBases.Length; i++)
        {
            runTargetTransform(cardMovementBases[i], i, angleStep, startAngle, rectTransform);
        }
    }

    private void runTargetTransform(CardMovementBase cardMovement, int index, float angleStep, float startAngle, RectTransform rectTransform)
    {
        float angle = startAngle + angleStep * index;
        float radian = angle * Mathf.Deg2Rad;
        float width = rectTransform.rect.width * 0.5f;

        cardMovement.TargetTransform = new()
        {
            Position = new Vector3(
                rectTransform.rect.center.x + Mathf.Sin(radian) * width,
                rectTransform.rect.min.y + Mathf.Cos(radian) * (width * HeightRatioFromWidth),
                0),
            Rotation = Quaternion.Euler(0, 0, -angle)
        };

        cardMovement.MoveCard();
    }
}
