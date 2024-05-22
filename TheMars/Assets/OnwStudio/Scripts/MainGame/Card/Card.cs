using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    public int No { get; internal set; }
    public string CardName { get; internal set; }
    public int MarsLithium { get; internal set; }
    public int People { get; internal set; }
    public CARD_KIND CardKind { get; internal set; }
    public CARD_GRADE CardGrade { get; internal set; }

    // .. 발동효과
    protected Dictionary<string, ICardEffect> _cardEffects = new();

    public void OnDraw()
    {

    }
}