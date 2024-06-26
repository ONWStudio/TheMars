using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using OnwAttributeExtensions;

public abstract class TMCardController<T> : MonoBehaviour where T : TMCardController<T>
{
    [field: SerializeField] public UnityEvent<T, bool> OnMoveToScreenCenter { get; private set; } = new();
    /// <summary>
    /// .. 카드가 무덤으로 이동해야 되는 경우 리스너들에게 알려주는 콜백 이벤트
    /// 카드 UI에서는 무덤으로 이동 할 방법을 알지못하기 때문에 무덤의 위치정보를 가지고 있는 객체가 카드 UI를 참조해서 알려주어야 함
    /// </summary>
    [field: SerializeField] public UnityEvent<T> OnMoveToTomb { get; private set; } = new();
    /// <summary>
    /// .. 카드가 사용 후 다시 패로 돌아가야 하는 경우 리스너들에게 알려주는 콜백 이벤트 입니다
    /// </summary>
    [field: SerializeField] public UnityEvent<T> OnRecycleToHand { get; private set; } = new();
    /// <summary>
    /// .. 카드가 드로우할때 사용되는 경우 리스너들에게 알려줍니다
    /// </summary>
    [field: SerializeField] public UnityEvent<T> OnDrawUse { get; private set; } = new();
    /// <summary>
    /// .. 카드가 파괴되는 경우 리스너들에게 알려줍니다
    /// </summary>
    [field: SerializeField] public UnityEvent<T> OnDestroyCard { get; private set; } = new();
    /// <summary>
    /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 시간
    /// </summary>
    [field: SerializeField] public UnityEvent<T, float> OnDelaySeconds { get; private set; } = new();
    /// <summary>
    /// .. 카드가 보유일 경우 리스너들에게 알려줍니다
    /// </summary>
    [field: SerializeField] public UnityEvent<T, string> OnHoldCard { get; private set; } = new();
    /// <summary>
    /// .. 카드가 소요일 경우 리스너들에게 알려줍니다 턴
    /// </summary>
    [field: SerializeField] public UnityEvent<T, int> OnDelayTurn { get; private set; } = new();

    /// <summary>
    /// .. 카드의 상세한 기본 데이터 입니다
    /// </summary>
    public TMCardData CardData
    {
        get => _cardData;
        set
        {
            if (_cardData) return;

            _cardData = value;
        }
    }

    [SerializeField, ReadOnly] protected TMCardData _cardData = null;

    public Action UseStartedState { get; set; } = null;
    public Action UseEndedState { get; set; } = null;
    public Action DrawBeginState { get; set; } = null;
    public Action DrawEndedState { get; set; } = null;
    public Action TurnEndedState { get; set; } = null;
}
