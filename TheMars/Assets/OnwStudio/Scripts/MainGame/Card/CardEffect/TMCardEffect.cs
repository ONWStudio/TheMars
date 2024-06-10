using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class TMCardData : ScriptableObject
{
    public interface ICardEffect
    {
        void OnEffect();
    }
}
