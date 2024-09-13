#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Onw.Editor.GUI;
using Onw.ScriptableObjects.Editor;
using TM.Synergy;
using static Onw.Editor.EditorGUIHelper;

namespace TMGuiTool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private sealed class TMSynergyDrawer : CustomInspectorEditorWindow, IGUIDrawer, IPagable
        {
            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Synergies";
            
            public int Page { get; set; }
            public int PrevPage { get; set; }
            public int MaxPage => _synergies.Count;

            public bool HasErrors { get; set; } = false;
            public bool IsSuccess { get; set; } = false;
            public string Message { get; set; } = string.Empty;

            private readonly List<TMSynergyData> _synergies = new();
            private readonly EditorScrollController _scrollViewController = new ();            
            
            public void Awake()
            {
                _synergies.AddRange(ScriptableObjectHandler<TMSynergyData>.LoadAllScriptableObjects());
                Page = 1;
            }

            public void OnEnable() {}

            public void OnDraw()
            {
                ActionEditorHorizontal(() =>
                {
                    if (!GUILayout.Button("새 시너지 추가")) return;
                    
                    _synergies.Add(ScriptableObjectHandler<TMSynergyData>.CreateScriptableObject(DATA_PATH, $"Synergy_No.{Guid.NewGuid().ToString()}"));
                });

                int page = Page - 1;
                if (page >= 0 && _synergies.Count > page)
                {
                    TMSynergyData synergyData = _synergies[page];
                    
                    _scrollViewController
                        .ActionScrollSpace(() => OnInspectorGUI(synergyData));
                }
            }
        }
    }
}
#endif
