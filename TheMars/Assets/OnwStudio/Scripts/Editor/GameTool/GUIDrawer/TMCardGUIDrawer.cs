#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMCard;
using Onw.Editor;
using Onw.Helpers;
using AddressableAssetBundleSpace;
using static Onw.Editor.EditorHelper;

namespace TMGUITool
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
                LoadDataFromLocal();
                Page = 1;
            }

            public void OnEnable() {}
            public void OnDraw()
            {
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
                    _scrollViewController
                        .ActionScrollSpace(() => OnInspectorGUI(_cards[page]));
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
                for (int i = 0; i < _cards.Count; i++)
                {
                    AddressableAssetBundleCreator
                        .CreateAssetBundle($"TMCard_{i + 1}", "TMCardData", _cards[i], "TMCardData");
                }
            }

            private void insertLocalizingOptions(TMCardData cardData)
            {

            }
        }
    }
}
#endif