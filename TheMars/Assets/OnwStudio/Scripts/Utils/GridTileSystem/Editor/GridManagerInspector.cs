#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;
using Onw.Editor.Extensions;
using Onw.Editor.GUI;
using Onw.Extensions;

namespace Onw.HexGrid.Editor
{
    using Editor = UnityEditor.Editor;
    using Event = UnityEngine.Event;
    
    [CustomEditor(typeof(GridManager))]
    internal sealed class GridManagerInspector : Editor
    {
        public readonly struct HexGUIData
        {
            public Vector3 TilePosition { get; }
            public string Description { get; }

            public HexGUIData(in Vector3 tilePosition, string description)
            {
                TilePosition = tilePosition;
                Description = description;
            }
        }

        private SerializedDictionary<string, HexGrid> _hexGrids = null;
        private readonly List<HexGUIData> _hexGridDescriptions = new();

        private GridManager _gridManager = null;
        private GUIStyle _style = null;
        private SerializedProperty _currentHexProperty = null;
        private int _tileCount = 0;

        private readonly SceneViewInnerWindow<GridManagerInspector> _currentHexWindow = new();


        private void OnEnable()
        {
            _gridManager = (target as GridManager)!;
            initializeTile();
            
            _currentHexWindow.OnEnable();
        }

        private void OnDisable()
        {
            _currentHexWindow.OnDisable();
        }

        private void initializeTile()
        {
            _tileCount = _gridManager.TileCount;
            _hexGrids = (_gridManager
                .GetType()
                .GetField("_hexGrids", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(_gridManager) as SerializedDictionary<string, HexGrid>)!;

            _hexGridDescriptions.Clear();
            _hexGridDescriptions.Capacity = _hexGrids.Count;
            _hexGridDescriptions.AddRange(_hexGrids
                .Select(kvp => new HexGUIData(
                    kvp.Value.TilePosition,
                    $"Q : {kvp.Value.Q} R : {kvp.Value.R} S : {kvp.Value.S} \n [{string.Join(", \n   ", kvp.Value.Properties)}]")));
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"TileCount : {_tileCount}");

            DrawDefaultInspector();

            if (GUILayout.Button("Calculate Grids"))
            {
                _gridManager.CalculateTile();
                initializeTile();
            }

            if (GUILayout.Button("Clear Tiles"))
            {
                _hexGrids.NewClear();
                initializeTile();
            }
        }

        private void OnSceneGUI()
        {
            Event currentEvent = Event.current;

            if (_currentHexProperty is not null)
            {
                _currentHexWindow.OnSceneGUI(windowId =>
                {
                    if (EditorGUILayout.PropertyField(_currentHexProperty))
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                });
            }
            
            if (currentEvent.type == EventType.MouseUp && _gridManager.TryGetTileDataByRay(HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition), out IHexGrid hexGrid))
            {
                int index = _hexGrids.Values.ToList().IndexOf((HexGrid)hexGrid);
                
                _currentHexProperty = serializedObject
                    .FindProperty("_hexGrids")
                    .FindPropertyRelative("_serializedList")
                    .GetArrayElementAtIndex(index)
                    .FindPropertyRelative("Value");
                    
                currentEvent.Use();
            }
            
            _style ??= new()
            {
                normal =
                {
                    textColor = Color.black
                },
                alignment = TextAnchor.MiddleCenter
            };

            foreach (HexGUIData hexMeta in _hexGridDescriptions)
            {
                Handles.BeginGUI();
                Vector3 guiPosition = HandleUtility.WorldToGUIPoint(hexMeta.TilePosition);
                GUI.Label(new(guiPosition.x - 25, guiPosition.y - 15, 50, 30), hexMeta.Description, _style);
                // GUI 그리기 종료
                Handles.EndGUI();
            }
        }
    }
}
#endif