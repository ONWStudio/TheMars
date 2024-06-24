using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardState
{
    void OnFire<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>;
}
