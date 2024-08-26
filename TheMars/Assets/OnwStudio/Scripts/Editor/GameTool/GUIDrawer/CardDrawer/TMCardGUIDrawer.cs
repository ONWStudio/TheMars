#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMCard;
using Onw.ScriptableObjects.Editor;
using Onw.Editor.GUI;
using static Onw.Editor.EditorGUIHelper;

namespace TMGuiTool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private sealed class TMCardGUIDrawer : CustomInspectorEditorWindow, IGUIDrawer, IPagable
        {
            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Cards";

            public int Page { get; set; }
            public int PrevPage { get; set; }
            public int MaxPage => _cards.Count;

            public bool HasErrors { get; set; } = false;
            public bool IsSuccess { get; set; } = false;
            public string Message { get; set; } = string.Empty;

            private readonly List<TMCardData> _cards = new();
            private readonly EditorScrollController _scrollViewController = new();

            public void Awake()
            {
                _cards.AddRange(ScriptableObjectHandler<TMCardData>.LoadAllScriptableObjects());
                Page = 1;
            }

            public void OnEnable() {}
            public void OnDraw()
            {
                ActionEditorHorizontal(() =>
                {
                    if (!GUILayout.Button("새 카드 추가")) return;

                    _cards.Add(ScriptableObjectHandler<TMCardData>.CreateScriptableObject(DATA_PATH, $"Card_No.{Guid.NewGuid().ToString()}"));
                });

                int page = Page - 1;
                if (page >= 0 && _cards.Count > page)
                {
                    TMCardData cardData = _cards[page];

                    _scrollViewController
                        .ActionScrollSpace(() => OnInspectorGUI(cardData));
                }
            }
        }
    }
}
#endif