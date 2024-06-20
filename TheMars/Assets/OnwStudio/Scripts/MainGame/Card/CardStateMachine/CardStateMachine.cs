using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// .. 특수 효과에 따라서 카드의 동작이 바뀌기 때문에 인터페이스를 통해 카드의 실제 구현된 객체에 접근하고 콜백 메서드로 동작을 정의합니다
/// </summary>
public class CardStateMachine
{
    public virtual void OnUseStarted<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        cardController.CardData.UseCard(cardController.gameObject);
    }

    public virtual void OnUseEnded<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        cardController.OnMoveToTomb.Invoke(cardController);
    }

    public virtual void OnTurnEnd<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        cardController.OnMoveToTomb.Invoke(cardController);
    }

    public virtual void OnDrawBegin<T>(T cardController) where T : MonoBehaviour, ITMCardController<T> {}
    public virtual void OnDrawEnded<T>(T cardController) where T : MonoBehaviour, ITMCardController<T> {}
}