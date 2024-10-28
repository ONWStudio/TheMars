#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Onw.Helper;
using Onw.Editor.GUI;
using Onw.Attribute;
using Onw.ScriptableObjects.Editor;
using TM.Event;
using static Onw.Editor.EditorGUIHelper;

namespace TMGuiTool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private sealed class TMEventDataDrawer : CustomInspectorEditorWindow, IGUIDrawer, IPagable
        {
            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Events";

            public int Page { get; set; }
            public int PrevPage { get; set; }
            public int MaxPage => _events.Count;

            public bool HasErrors { get; set; } = false;
            public bool IsSuccess { get; set; } = false;
            public string Message { get; set; } = string.Empty;

            private readonly List<TMEventData> _events = new();
            private readonly EditorScrollController _scrollViewController = new();
            private readonly EditorDropdownController<Type> _eventDataSubTypesDropdown = new(
                "새 이벤트 추가",
                ReflectionHelper.GetChildClassesFromBaseType(typeof(TMEventData))
                    .ToDictionary(t => t.GetCustomAttribute<SubstitutionAttribute>()?.Label ?? t.Name, t => t));
            
            public void Awake()
            {
                TMEventData[] events = ScriptableObjectHandler
                    .LoadAllScriptableObjects<TMEventData>()
                    .ToArray();

                TMEventData[] unmadeTypes = _eventDataSubTypesDropdown
                    .Options
                    .Values
                    .Where(t => events.All(evt => t != evt.GetType()))
                    .Select(t => ScriptableObjectHandler.CreateScriptableObject(DATA_PATH, $"Event_No.{Guid.NewGuid().ToString()}", t))
                    .OfType<TMEventData>()
                    .ToArray();
                
                _events.AddRange(events.Concat(unmadeTypes));
                Page = 1;
            }

            public void OnEnable() { }

            public void OnDraw()
            {
                ActionEditorHorizontal(() =>
                {
                    _eventDataSubTypesDropdown.Dropdown(type =>
                        _events.Add(
                            ScriptableObjectHandler.CreateScriptableObject(
                                DATA_PATH, 
                                $"Event_No.{Guid.NewGuid().ToString()}", 
                                type) as TMEventData));
                });

                int page = Page - 1;
                if (page >= 0 && _events.Count > page)
                {
                    TMEventData eventData = _events[page];
                    SubstitutionAttribute substitutionAttribute = eventData.GetType().GetCustomAttribute<SubstitutionAttribute>();

                    if (substitutionAttribute is not null)
                    {
                        EditorGUILayout.LabelField(substitutionAttribute.Label);
                    }
                    
                    _scrollViewController
                        .ActionScrollSpace(() => OnInspectorGUI(eventData));
                }
            }
        }
    }
}
#endif