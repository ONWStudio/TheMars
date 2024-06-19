#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;
using static EditorTool.EditorTool;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
    {
        private sealed class CardGUIDrawer : IGUIDrawer
        {
            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Cards";
            private Editor _editor = null;

            public int Page { get; set; }
            public int MaxPage => _cards.Count;

            public bool HasErrors { get; set; } = false;
            public bool IsSuccess { get; set; } = false;
            public string Message { get; set; } = string.Empty;

            private readonly List<TMCardData> _cards = new();
            private readonly EditorScrollController _scrollViewController = new();

            public void Awake()
            {
                LoadDataFromLocal();
                Page = 1;
            }

            public void OnEnable()
            {

            }

            public void OnDraw()
            {
                TMCardData cardData = null;

                ActionEditorHorizontal(() =>
                {
                    if (!GUILayout.Button("새 카드 추가")) return;

                    _cards.Add(DataHandler<TMCardData>.CreateScriptableObject(DATA_PATH, $"Card_No.{_cards.Count + 1}"));
                    insertLocalizingOptions(_cards[^1]);
                    DataHandler<TMCardData>.SaveData(_cards[^1]);
                });

                int page = Page - 1;

                if (page >= 0 && _cards.Count > page)
                {
                    _scrollViewController.ActionScrollSpace(() =>
                    {
                        cardData = _cards[page];

                        if (_editor == null || _editor.target != cardData)
                        {
                            _editor = Editor.CreateEditor(cardData);
                        }

                        _editor.OnInspectorGUI();
                    });
                }
            }

            public void LoadDataFromLocal()
            {
                _cards.Clear();
                _cards.AddRange(DataHandler<TMCardData>
                    .LoadAllScriptableObjects());

                _cards.ForEach(insertLocalizingOptions);
            }

            public void SaveDataToLocal()
            {

            }

            private void insertLocalizingOptions(TMCardData cardData)
            {

            }
        }
    }
}
#endif