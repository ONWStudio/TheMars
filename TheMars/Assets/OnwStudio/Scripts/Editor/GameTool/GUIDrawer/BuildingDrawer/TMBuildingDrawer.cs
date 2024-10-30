// #if UNITY_EDITOR
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Onw.Editor.GUI;
// using Onw.ScriptableObjects.Editor;
// using TM.Building;
// using UnityEditor;
// using UnityEngine;
// using static Onw.Editor.EditorGUIHelper;
//
// namespace TMGuiTool
// {
//     internal sealed partial class GUIToolDrawer : EditorWindow
//     {
//         private sealed class TMBuildingDrawer : CustomInspectorEditorWindow, IGUIDrawer, IPagable
//         {
//             private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Buildings";
//             
//             public int Page { get; set; }
//             public int PrevPage { get; set; }
//             public int MaxPage => _buildings.Count;
//             
//             public bool HasErrors { get; set; }
//             public bool IsSuccess { get; set; }
//             public string Message { get; set; } = string.Empty;
//
//             private readonly List<TMBuildingData> _buildings = new();
//             private readonly EditorScrollController _scrollViewController = new();
//
//             public void Awake()
//             {
//                 _buildings.AddRange(ScriptableObjectHandler.LoadAllScriptableObjects<TMBuildingData>());
//                 Page = 1;
//             }
//
//             public void OnEnable() {}
//
//             public void OnDraw()
//             {
//                 ActionEditorHorizontal(() =>
//                 {
//                     if (!GUILayout.Button("새 건물 추가")) return;
//                     
//                     _buildings.Add(ScriptableObjectHandler.CreateScriptableObject<TMBuildingData>(DATA_PATH, $"Building_No.{Guid.NewGuid().ToString()}"));
//                 });
//
//                 int page = Page - 1;
//                 if (page >= 0 && _buildings.Count > page)
//                 {
//                     TMBuildingData buildingData = _buildings[page];
//                     
//                     _scrollViewController.ActionScrollSpace(() => OnInspectorGUI(buildingData));
//                 }
//             }
//         }
//     }
// }
// #endif