using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ElectricCondition : ICardCondition
{
    [field: SerializeField] public int Electric { get; internal set; } = 0;
    public bool AdditionalCondition => true;
}
