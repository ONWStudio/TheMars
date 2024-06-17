using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITMCardController<T> where T : MonoBehaviour, ITMCardController<T>
{
    public TMCardData CardData { get; }
    /// <summary>
    /// .. 카드를 사용할때 (연출효과) 호출되는 콜백 이벤트
    /// </summary>
    public UnityEvent<T> OnUseStarted { get; }
    /// <summary>
    /// .. 카드의 사용 후 (연출효과) 호출되는 콜백 이벤트 
    /// </summary>
    public UnityEvent<T> OnUseEnded { get; }
    /// <summary>
    /// .. 카드가 무덤으로 이동해야 되는 경우 리스너들에게 알려주는 콜백 이벤트
    /// 카드 UI에서는 무덤으로 이동 할 방법을 알지못하기 때문에 무덤의 위치정보를 가지고 있는 객체가 카드 UI를 참조해서 알려주어야 함
    /// </summary>
    public UnityEvent<T> OnMoveToTomb { get; }
    /// <summary>
    /// .. 카드가 사용 후 다시 패로 돌아가야 하는 경우 리스너들에게 알려주는 콜백 이벤트 입니다
    /// </summary>
    public UnityEvent<T> OnRecycleToHand { get; }
    /// <summary>
    /// .. 카드가 드로우 일 경우 리스너들에게 알려줍니다
    /// </summary>
    public UnityEvent<T> OnDrawUse { get; }
    /// <summary>
    /// .. 카드가 파괴되는 경우 리스너들에게 알려줍니다
    /// </summary>
    public UnityEvent<T> OnDestroyCard { get; }
    /// <summary>
    /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 시간
    /// </summary>
    public UnityEvent<T, float> OnDelaySeconds { get; }
    /// <summary>
    /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 턴
    /// </summary>
    public UnityEvent<T, int> OnDelayTurn { get; }
    /// <summary>
    /// .. 카드가 보유일 경우 리스너들에게 알려줍니다
    /// </summary>
    public UnityEvent<T, int> OnHoldCard { get; }
}
