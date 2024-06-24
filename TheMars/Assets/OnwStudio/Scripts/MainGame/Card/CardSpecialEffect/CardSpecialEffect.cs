using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// .. 특수 효과에 따라서 카드의 동작이 바뀌기 때문에 인터페이스를 통해 카드의 실제 구현된 객체에 접근하고 콜백 메서드로 동작을 정의합니다
/// </summary>
public interface ICardSpecialEffect
{
    void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>;

    /// <summary>
    /// .. 카드의 특수효과 중 함께 있을 수 없는 카드효과들을 정의합니다
    /// </summary>
    /// <param name="cardSpecialEffects"></param>
    /// <returns></returns>
    bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects);
}