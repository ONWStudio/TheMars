using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ScriptableObjects
{
    public sealed class NumberReference : ScriptableObject
    {
        [field: SerializeField] public int No { get; }
    }
}