#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMCard.Manager;
using Onw.Manager;
using Onw.Editor.GUI;
using Michsky.UI.Heat;
using System;

namespace TMGUITool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private sealed class TMLocalizationDrawer : CustomInspectorEditorWindow, IGUIDrawer
        {
            private int _selectedTab = 0;
            private string[] _tabs = null;

            public bool HasErrors { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }

            private readonly EditorScrollController _scrollViewController = new();
            private readonly List<LocalizationTable> _localizationTables = new();

            public void Awake()
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type baseType = typeof(LocalizationSingleton<>);

                Debug.Log(typeof(TMSpecialEffectNameTable).BaseType.GetGenericTypeDefinition());

                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.BaseType is null || !type.BaseType.IsGenericType || type.BaseType.GetGenericTypeDefinition() != baseType.GetGenericTypeDefinition()) continue;
                        Debug.Log(type);

                        var instances = Resources.FindObjectsOfTypeAll(type);
                        _localizationTables.AddRange(instances.Cast<LocalizationTable>());
                    }
                }

                _tabs = _localizationTables
                    .Select(localizationTable => localizationTable.tableID)
                    .ToArray();
            }

            public void OnEnable() { }

            public void OnDraw()
            {
                if (_localizationTables.Count <= 0) return;

                _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs);

                _scrollViewController
                    .ActionScrollSpace(() => OnInspectorGUI(_localizationTables[_selectedTab]));
            }
        }
    }
}
#endif