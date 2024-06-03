using System.Collections;
using System.Collections.Generic;
using TcgEngine;
using UnityEngine;

public sealed class SectorForm : ICardSorter
{
    private const float MAX_ANGLE = 128f;

    public void BatchCard(CardMovementBase[] cardMovementBases, int index, Vector3 pivot, float width)
    {
        if (cardMovementBases.Length <= 0 || cardMovementBases.Length <= index) return;

        float angleStep = MAX_ANGLE / (cardMovementBases.Length - 1);
        float startAngle = -(MAX_ANGLE / 2);

        float angle = startAngle + angleStep * index;
        float radian = (angle + 90f) * Mathf.Deg2Rad;

        float x = pivot.x + Mathf.Cos(radian) * width;
        float y = pivot.y + Mathf.Sin(radian) * (width * 0.35f);

        cardMovementBases[index].TargetTransform = new()
        {
            Position = new Vector3(x, y, 0),
            Rotation = Quaternion.Euler(0, 0, angle)
        };

        cardMovementBases[index].MoveCard();
    }

    public void SortCards(CardMovementBase[] cardMovementBases, Vector3 pivot, float width)
    {
        float angleStep = MAX_ANGLE / (cardMovementBases.Length - 1);
        float startAngle = -(MAX_ANGLE / 2);

        for (int i = 0; i < cardMovementBases.Length; i++)
        {
            float angle = startAngle + angleStep * i;
            float radian = (angle + 90f) * Mathf.Deg2Rad;

            float x = pivot.x + Mathf.Cos(radian) * width;
            float y = pivot.y + Mathf.Sin(radian) * (width * 0.35f);

            cardMovementBases[i].TargetTransform = new()
            {
                Position = new Vector3(x, y, 0),
                Rotation = Quaternion.Euler(0, 0, angle)
            };

            cardMovementBases[i].MoveCard();
        }
    }
}
