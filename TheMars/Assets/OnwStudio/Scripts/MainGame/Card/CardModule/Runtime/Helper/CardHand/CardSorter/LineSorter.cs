using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Extensions;
using Onw.UI;

namespace TMCard.Runtime
{
    public sealed class LineSorter : ICardSorter
    {
        public List<PositionRotationInfo> SortCards(List<TMCardController> cards, RectTransform rectTransform)
        {
            return cards.Select((t, i) => getPositionRotationInfo(cards, i, rectTransform)).ToList();
        }
        
        public PositionRotationInfo ArrangeCard(List<TMCardController> cards, int index, RectTransform rectTransform)
        {
            if (index < 0 || index >= cards.Count) return null;

            return getPositionRotationInfo(cards, index, rectTransform);
        }

        private static PositionRotationInfo getPositionRotationInfo(List<TMCardController> cards, int index, RectTransform rectTransform)
        {
            TMCardController card = cards[index];

            float width = rectTransform.rect.width;
            float normalizedValue = (float)(index + 1) / (cards.Count + 1);

            float positionX = width * normalizedValue;
            Vector2 worldPoint = rectTransform.TransformPoint(new(positionX, 0, 0));
            Vector2 boardPoint = rectTransform.parent.InverseTransformPoint(worldPoint);
            
            return new(card, new(boardPoint.x - rectTransform.rect.width * 0.5f, 0), Vector3.zero);
        }
    }
}
