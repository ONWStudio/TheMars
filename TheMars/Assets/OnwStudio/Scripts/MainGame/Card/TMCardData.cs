using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class TMCardData : ScriptableObject
{
    public int No { get; internal set; }
    public string CardName { get; internal set; }
    public int MarsLithium { get; internal set; }
    public int People { get; internal set; }
    public CARD_KIND CardKind { get; internal set; }
    public CARD_GRADE CardGrade { get; internal set; }

    public List<ICardEffect> CardEffects { get; internal set; } = new();
}