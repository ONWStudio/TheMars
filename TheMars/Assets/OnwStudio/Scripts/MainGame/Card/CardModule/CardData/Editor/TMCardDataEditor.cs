#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using NaughtyAttributes;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        [Button("Generate GUID")]
        private void generateNewGUID()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}
#endif