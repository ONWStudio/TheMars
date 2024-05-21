using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    [Serializable]
    public sealed class Energy
    {
        [field: SerializeField] public uint MarsLithium { get; private set; }
        [field: SerializeField] public uint People { get; private set; }

        protected Energy() {}
    }
}
