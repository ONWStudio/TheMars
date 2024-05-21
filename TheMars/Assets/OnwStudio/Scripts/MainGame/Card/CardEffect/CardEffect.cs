using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    public interface ICardEffect
    {
        void OnEffect();
    }
}
