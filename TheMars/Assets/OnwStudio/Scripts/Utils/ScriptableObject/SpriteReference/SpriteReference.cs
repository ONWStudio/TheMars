using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ScritableObjects
{
    [CreateAssetMenu(fileName = "SpriteReference", menuName = "Scriptable Object/Sprite Reference")]
    public sealed class SpriteReference : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
