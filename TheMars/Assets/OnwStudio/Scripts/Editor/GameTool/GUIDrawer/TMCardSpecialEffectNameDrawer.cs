#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMCard.Manager;
using Onw.Editor.GUI;

namespace TMGUITool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private sealed class TMCardSpecialEffectNameDrawer : CustomInspectorEditorWindow, IGUIDrawer
        {
            public bool HasErrors { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }

            private readonly EditorScrollController _scrollViewController = new();
            public void Awake() {}
            public void OnEnable() {}

            public void OnDraw()
            {
                _scrollViewController
                    .ActionScrollSpace(() => OnInspectorGUI(TMSpecialEffectNameManager.Instance));
            }
        }
    }
}
#endif