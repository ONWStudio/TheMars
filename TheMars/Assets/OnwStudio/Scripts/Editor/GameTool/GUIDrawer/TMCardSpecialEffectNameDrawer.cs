#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMCard.Manager;

namespace TMGUITool
{
    internal sealed partial class TMGUIToolDrawer : EditorWindow
    {
        private sealed class TMCardSpecialEffectNameDrawer : IGUIDrawer
        {
            public int Page { get; set; }

            public int MaxPage => 0;

            public bool HasErrors { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }

            public void Awake()
            {
                _ = TMSpecialEffectNameManager.Instance;
            }

            public void LoadDataFromLocal()
            {
            }

            public void OnDraw()
            {
            }

            public void OnEnable()
            {
            }

            public void SaveDataToLocal()
            {
            }
        }
    }
}
#endif