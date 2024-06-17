using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2RangeSpace;

namespace TMCardUISystemModules
{
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
        /// <param name="cardUIs"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
        /// <param name="index"> .. 배치시킬 카드의 인덱스 </param>
        /// <param name="rectTransform"> .. 반지름의 기준이 될 렉트 트랜스폼 입니다 </param>
        public void ArrangeCard(List<TMCardUIController> cardUIs, int index, RectTransform rectTransform)
        {
            if (cardUIs.Count <= 0 || cardUIs.Count <= index) return;

            float angleStep = MaxAngle / (cardUIs.Count > 1 ? (cardUIs.Count - 1) : 2);
            float startAngle = -MaxAngle * 0.5f;

            runTargetTransform(cardUIs[index], cardUIs.Count > 1 ? index : 1, angleStep, startAngle, rectTransform);
        }

        /// <summary>
        /// .. 카드들을 올바른 위치로 배치 시킵니다
        /// </summary>
        /// <param name="cardUIs"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다</param>
        /// <param name="rectTransform"> .. 반지름의 기준이 될 렉트 트랜스폼 입니다 </param>
        public void SortCards(List<TMCardUIController> cardUIs, RectTransform rectTransform)
        {
            float angleStep = MaxAngle / (cardUIs.Count > 1 ? (cardUIs.Count - 1) : 2);
            float startAngle = -MaxAngle * 0.5f;

            for (int i = 0; i < cardUIs.Count; i++)
            {
                runTargetTransform(cardUIs[i], cardUIs.Count > 1 ? i : 1, angleStep, startAngle, rectTransform);
            }
        }

        private void runTargetTransform(TMCardUIController cardUI, int index, float angleStep, float startAngle, RectTransform rectTransform)
        {
            float angle = startAngle + angleStep * index;
            float radian = angle * Mathf.Deg2Rad;
            float radius = rectTransform.rect.height * RadiusRatioOffset;
            Vector2 pivot = new(rectTransform.rect.center.x, rectTransform.rect.min.y);

            Vector3 targetPosition = new(
            pivot.x + (rectTransform.rect.width * PositionRatioOffset.x) + Mathf.Sin(radian) * radius,
            pivot.y + (rectTransform.rect.height * PositionRatioOffset.y) + Mathf.Cos(radian) * radius,
            0f);

            MMF_Parallel feedbackParallel = new();

            feedbackParallel.Feedbacks.Add(EventCreator
                .CreateSmoothPositionEvent(cardUI.gameObject, targetPosition));

            feedbackParallel.Feedbacks.Add(EventCreator
                .CreateSmoothRotationEvent(cardUI.transform, new(0f, 0f, -angle)));

            cardUI.EventSender.PlayEvent(feedbackParallel);
        }
    }
}
