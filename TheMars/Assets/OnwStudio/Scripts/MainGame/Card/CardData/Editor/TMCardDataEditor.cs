#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OnwAttributeExtensions;

public sealed partial class TMCardData : ScriptableObject
{
    [InspectorButton("Generate GUID")]
    private void generateNewGUID()
    {
        Guid = System.Guid.NewGuid().ToString();
    }
}
#endif