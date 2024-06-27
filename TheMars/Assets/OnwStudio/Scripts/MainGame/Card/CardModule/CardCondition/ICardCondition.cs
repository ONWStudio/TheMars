using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardCondition
{
    bool AdditionalCondition { get; }
}

public interface ICardCondtionProcessing
{
    void UseProcessing();
}
