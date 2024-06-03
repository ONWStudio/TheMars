using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardSorter
{
    void SortCards(CardMovementBase[] cardMovementBases, Vector3 pivot, float width);
    void BatchCard(CardMovementBase[] cardMovementBases, int index, Vector3 pivot, float width);
}
