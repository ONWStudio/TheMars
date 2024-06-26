using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCardUISystemModules
{
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
        public void ArrangeCard(List<TMCardUIController> cardUIs, int index, RectTransform rectTransform, float duration = 1.0f)
        {
            if (cardUIs.Count <= 0 || cardUIs.Count <= index) return;

            float angleStep = MaxAngle / (cardUIs.Count > 1 ? (cardUIs.Count - 1) : 2);
            float startAngle = -MaxAngle * 0.5f;

            runTargetTransform(cardUIs[index], cardUIs.Count > 1 ? index : 1, angleStep, startAngle, rectTransform, duration);
        }

        /// <summary>
        /// .. 카드들을 올바른 위치로 배치 시킵니다
        /// </summary>
        /// <param name="cardUIs"> .. 카드의 움직임을 정하는 무브먼트 클래스를 받습니다 </param>
        /// <param name="rectTransform"> .. 비율로 배치시키기 때문에 기준이 되는 렉트 트랜스폼을 인자값으로 넣어줍니다 </param> 
        public void SortCards(List<TMCardUIController> cardUIs, RectTransform rectTransform, float duration = 1.0f)
        {
            float angleStep = MaxAngle / (cardUIs.Count > 1 ? (cardUIs.Count - 1) : 2);
            float startAngle = -MaxAngle * 0.5f;

            for (int i = 0; i < cardUIs.Count; i++)
            {
                runTargetTransform(cardUIs[i], cardUIs.Count > 1 ? i : 1, angleStep, startAngle, rectTransform, duration);
            }
        }

        private void runTargetTransform(TMCardUIController cardUI, int index, float angleStep, float startAngle, RectTransform rectTransform, float duration)
        {
            float angle = startAngle + angleStep * index;
            float radian = angle * Mathf.Deg2Rad;
            float width = rectTransform.rect.width * 0.5f;

            Vector3 targetPosition = new(
                    rectTransform.rect.center.x + Mathf.Sin(radian) * width,
                    rectTransform.rect.min.y + Mathf.Cos(radian) * (width * HeightRatioFromWidth),
                    0);

            cardUI.EventSender.PlayEvent(
                EventCreator
                    .CreateSmoothPositionAndRotationEvent(cardUI.gameObject, targetPosition, new(0f, 0f, -angle), duration));
        }
    }
}
