using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2RangeSpace;

/// <summary>
/// .. 카드를 원 형태로 정렬시키는 클래스
/// </summary>
public sealed class CycleForm : ICardSorter
{
    /// <summary>
    /// .. 최대 앵글 값
    /// </summary>
    [field: SerializeField, Range(20f, 180f)] public float MaxAngle { get; set; } = 90f;
    [field: SerializeField, Range(0f, 2f)] public float RadiusRatioOffset { get; set; } = 0.5f;
    [field: SerializeField, Vector2Range(-1f, 2f)] public Vector2 PositionRatioOffset { get; set; } = new Vector2(0f, 0f);

    /// <summary>
    /// .. 특정 인덱스의 카드를 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="index"> .. 배치시킬 카드의 인덱스 </param>
    /// <param name="rectTransform"> .. 반지름의 기준이 될 렉트 트랜스폼 입니다 </param>
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
    /// <param name="cardMovementBases"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다</param>
    /// <param name="rectTransform"> .. 반지름의 기준이 될 렉트 트랜스폼 입니다 </param>
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
        float radius = rectTransform.rect.height * RadiusRatioOffset;
        Vector2 pivot = new(rectTransform.rect.center.x, rectTransform.rect.min.y);

        cardMovement.TargetTransform = new()
        {
            Position = new Vector3(
                pivot.x + (rectTransform.rect.width * PositionRatioOffset.x) + Mathf.Sin(radian) * radius,
                pivot.y + (rectTransform.rect.height * PositionRatioOffset.y) + Mathf.Cos(radian) * radius,
                0),
            Rotation = Quaternion.Euler(0, 0, -angle)
        };
    }
}
