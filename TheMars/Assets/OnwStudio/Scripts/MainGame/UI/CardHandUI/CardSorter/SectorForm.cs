using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
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
    [field: SerializeField, Range(20f, 180f)] public float MaxAngle { get; set; } = 128f;
    /// <summary>
    /// .. 높이 비율
    /// </summary>
    [field: SerializeField, Range(0.1f, 0.5f)] public float HeightRatioFromWidth { get; set; } = 0.25f;

    /// <summary>
    /// .. 특정 인덱스의 카드를 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardUIs"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="index"> .. 배치시킬 카드의 인덱스 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    public void ArrangeCard(TMCardUI[] cardUIs, int index, RectTransform rectTransform)
    {
        if (cardUIs.Length <= 0 || cardUIs.Length <= index) return;

        float angleStep = MaxAngle / (cardUIs.Length - 1);
        float startAngle = -MaxAngle * 0.5f;

        SortCards(cardUIs[index], index, angleStep, startAngle, rectTransform);
    }

    /// <summary>
    /// .. 카드들을 올바른 위치로 배치 시킵니다
    /// </summary>
    /// <param name="cardUIs"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
    /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
    public void SortCards(TMCardUI[] cardUIs, RectTransform rectTransform)
    {
        float angleStep = MaxAngle / (cardUIs.Length - 1);
        float startAngle = -MaxAngle * 0.5f;

        for (int i = 0; i < cardUIs.Length; i++)
        {
            SortCards(cardUIs[i], i, angleStep, startAngle, rectTransform);
        }
    }

    private void SortCards(TMCardUI cardUI, int index, float angleStep, float startAngle, RectTransform rectTransform)
    {
        float angle = startAngle + angleStep * index;
        float radian = angle * Mathf.Deg2Rad;
        float width = rectTransform.rect.width * 0.5f;

        Vector3 targetPosition = new(
                rectTransform.rect.center.x + Mathf.Sin(radian) * width,
                rectTransform.rect.min.y + Mathf.Cos(radian) * (width * HeightRatioFromWidth),
                0);

        Quaternion targetRotation = Quaternion.Euler(0, 0, -angle);

        AnimationCurve logCurve = AnimationCurveLoader.Instance.AnimationCurves["LogCurve"];

        MMF_Position mmfPosition = new()
        {
            Label = "Position",
            AnimatePositionTarget = cardUI.gameObject,
            DestinationPosition = targetPosition,
            Mode = MMF_Position.Modes.ToDestination,
            Space = MMF_Position.Spaces.Local,
            FeedbackDuration = 0.5f,
            RelativePosition = false,
            AnimatePositionCurveX = logCurve,
            AnimatePositionCurveY = logCurve,
            AnimatePositionCurveZ = logCurve,
            AnimatePositionCurve = logCurve
        };

        MMF_Rotation mmfRotation = new()
        {
            Label = "Rotation",
            AnimateRotationTarget = cardUI.transform,
            DestinationAngles = targetRotation.eulerAngles,
            Mode = MMF_Rotation.Modes.ToDestination,
            RotationSpace = Space.Self,
            FeedbackDuration = 0.5f,
            AnimateRotationX = logCurve,
            AnimateRotationY = logCurve,
            AnimateRotationZ = logCurve
        };

        cardUI.EventReceiver.PlayEvent(mmfPosition, mmfRotation);
    }
}
