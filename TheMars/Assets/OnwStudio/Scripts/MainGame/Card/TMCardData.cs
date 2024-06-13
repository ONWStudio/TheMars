using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

/// <summary>
/// .. 카드의 랜덤
/// </summary>
public sealed class TMCardData : ScriptableObject
{
    private const int DEFAULT_EFFECT = 0;
    private const int DROP_EFFECT = 1;

    public int No { get; internal set; }
    public string CardName { get; internal set; }
    public int MarsLithium { get; internal set; }
    public int People { get; internal set; }
    public CARD_KIND CardKind { get; internal set; }
    public CARD_GRADE CardGrade { get; internal set; }
    public CARD_SPECIAL_EFFECT CardSpecialEffect { get; internal set; } = CARD_SPECIAL_EFFECT.NONE;

    public List<List<ICardEffect>> CardEffects { get; internal set; } = new() { new List<ICardEffect>() };

    public void UseCard(GameObject gameObject)
    {
        CardEffects[0].ForEach(cardEffect => cardEffect.OnEffect(gameObject, this));
    }
}