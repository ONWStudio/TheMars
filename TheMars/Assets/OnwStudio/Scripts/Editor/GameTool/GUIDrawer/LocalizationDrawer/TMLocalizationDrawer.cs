#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Manager;
using Onw.Editor;
using Onw.Editor.GUI;
using Michsky.UI.Heat;

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
                Type baseType = typeof(LocalizationSingleton<>);

                var tables = EditorReflectionHelper
                    .GetSubclassesOfGenericClass<LocalizationTable>(baseType)
                    .ToArray();

                BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
                foreach (Type subType in EditorReflectionHelper.GetSubclassTypeFromGenericType(baseType))
                {
                    if (tables.Any(table => table.GetType() == subType)) continue;

                    // .. 인스턴스가 존재하지 않을 경우 ..
                    PropertyInfo property = subType.BaseType.GetProperty("Instance", flags);
                    if (property.GetValue(null) is LocalizationTable localizationTable)
                    {
                        _localizationTables.Add(localizationTable);
                    }
                }

                _localizationTables.AddRange(tables);

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