#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;
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

        private SerializedDictionary<AxialCoordinates, HexGrid> _hexGrids = null;
        private readonly List<HexGUIData> _hexGridDescriptions = new();

        private GridManager _gridManager = null;
        private GUIStyle _style = null;
        private SerializedProperty _currentHexProperty = null;
        private SceneViewInnerWindow<GridManagerInspector> _hexOptionWindow = new();
        private Vector2 _scrollPosition = Vector2.zero;
        private int _tileCount = 0;

        private void OnEnable()
        {
            _gridManager = (target as GridManager)!;
            initializeTile();
            _hexOptionWindow.OnEnable();
        }

        private void OnDisable()
        {
            _hexOptionWindow.OnDisable();
            _currentHexProperty = null;
        }

        private void initializeTile()
        {
            _tileCount = _gridManager.TileCount;
            _hexGrids = (_gridManager
                .GetType()
                .GetField("_hexGrids", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(_gridManager) as SerializedDictionary<AxialCoordinates, HexGrid>)!;

            _hexGridDescriptions.Clear();
            _hexGridDescriptions.Capacity = _hexGrids.Count;
            _hexGridDescriptions.AddRange(_hexGrids
                .Select(kvp => new HexGUIData(
                    kvp.Value.TilePosition,
                    $"Q : {kvp.Value.HexPoint.Q} R : {kvp.Value.HexPoint.R} S : {kvp.Value.HexPoint.S} \n [{string.Join(", \n   ", kvp.Value.Properties)}]")));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"TileCount : {_tileCount}");

            DrawDefaultInspector();

            if (GUILayout.Button("Calculate Grids"))
            {
                Undo.RecordObject(_gridManager, "Calculate Grids");
                _gridManager.CalculateTile();
                initializeTile();
                serializedObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Update Option"))
            {
                _gridManager.SendToShaderHexOption();
                serializedObject.ApplyModifiedProperties();
            }
            
            if (GUILayout.Button("Clear Tiles"))
            {
                _hexGrids.NewClear();
                initializeTile();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {
            _style ??= new()
            {
                normal =
                {
                    textColor = Color.black
                },
                alignment = TextAnchor.MiddleCenter
            };

            Event currentEvent = Event.current;
            if (_currentHexProperty is not null)
            {
                _hexOptionWindow.OnSceneGUI(id =>
                {
                    using EditorGUILayout.ScrollViewScope scrollScope = new(_scrollPosition);
                    _scrollPosition = scrollScope.scrollPosition;

                    using EditorGUI.ChangeCheckScope changeCheckScope = new();
                    EditorGUILayout.PropertyField(_currentHexProperty, true);
                    
                    if (changeCheckScope.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                });
            }
            
            if (!_hexOptionWindow.IsUse && currentEvent.type == EventType.MouseUp && _gridManager.TryGetTileDataByRay(HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition), out IHexGrid hexGrid))
            {
                int index = _hexGrids.Values.ToList().IndexOf((HexGrid)hexGrid);

                _currentHexProperty = serializedObject
                    .FindProperty("_hexGrids")
                    .FindPropertyRelative("_serializedList")
                    .GetArrayElementAtIndex(index)
                    .FindPropertyRelative("Value");

                _currentHexProperty.isExpanded = true;
                currentEvent.Use();
            }
            
            Handles.BeginGUI();
            foreach (HexGUIData hexMeta in _hexGridDescriptions)
            {
                Vector3 guiPosition = HandleUtility.WorldToGUIPoint(hexMeta.TilePosition);
                GUI.Label(new(guiPosition.x - 25, guiPosition.y - 15, 50, 30), hexMeta.Description, _style);
                // GUI 그리기 종료
            }
            Handles.EndGUI();
        }
    }
}
#endif