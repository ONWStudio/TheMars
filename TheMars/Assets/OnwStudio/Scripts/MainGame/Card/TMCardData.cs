using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 카드의 랜덤
/// </summary>
public sealed class TMCardData : ScriptableObject
{
    /// <summary>
    /// .. 카드의 고유 ID 또는 스택 ID
    /// </summary>
    public int No { get; internal set; }
    /// <summary>
    /// .. 카드의 고유 이름
    /// </summary>
    public string CardName { get; internal set; }
    /// <summary>
    /// .. 마르스 리튬
    /// </summary>
    public int MarsLithium { get; internal set; }
    /// <summary>
    /// .. 인구 수
    /// </summary>
    public int People { get; internal set; }
    /// <summary>
    /// .. 카드의 종류
    /// </summary>
    public CARD_KIND CardKind { get; internal set; }
    /// <summary>
    /// .. 카드의 등급
    /// </summary>
    public CARD_GRADE CardGrade { get; internal set; }
    public List<ICardEffect> CardEffects { get; internal set; } = new();
    /// <summary>
    /// .. 카드의 특수효과입니다
    /// CardStateMachine는 ITMCardController 상속받은 실제 카드 구현체를 바인딩하여 필요한 기능을 구현합니다
    /// </summary>
    public CardStateMachine StateMachine { get; internal set; } = new();

    /// <summary>
    /// .. 발동 효과 입니다
    /// </summary>
    /// <param name="gameObject"></param>
    public void UseCard(GameObject gameObject)
    {
        CardEffects.ForEach(cardEffect => cardEffect.OnEffect(gameObject, this));
    }
}