using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    public int No { get; private set; }
    public Energy NecessaryEnergy { get; private set; }
    public string Name { get; }
    public CARD_KIND CardKind { get; }
    public CARD_GRADE CardGrade { get; }

    // .. 발동효과
    protected Dictionary<string, ICardEffect> _cardEffects = new();

    public void OnDraw()
    {

    }
}