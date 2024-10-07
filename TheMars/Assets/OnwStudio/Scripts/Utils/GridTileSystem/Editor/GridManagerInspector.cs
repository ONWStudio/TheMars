#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Onw.Extensions;
using UnityEngine;
using UnityEditor;

namespace Onw.HexGrid.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(GridManager))]
    internal sealed class GridManagerInspector : Editor
    {
        private readonly List<HexGrid> _tileList = new();
        private GridManager _gridManager = null;
        private GUIStyle _style = null;
        private int _tileCount = 0;

        private void OnEnable()
        {
            _gridManager = target as GridManager;
            initializeTile();
        }

        private void initializeTile()
        {
            _gridManager = target as GridManager;
            _tileCount = _gridManager.TileCount;
            _tileList.AddRange((_gridManager!
                    .GetType()
                    .GetField("_tileList", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(_gridManager) as List<HexGrids>)!
                .SelectMany(grids => grids.Grids));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"TileCount : {_tileCount}");
            
            DrawDefaultInspector();

            if (GUILayout.Button("Calculate Grids"))
            {
                
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

            // foreach (GridRows rows in _tileList)
            // {
            //     foreach (HexGrid tile in rows.Row)
            //     {
            //         Vector3 tilePosition = tile.transform.position;
            //
            //         // Scene 뷰에서 3D 공간의 위치를 GUI 좌표로 변환
            //         Vector2 guiPosition = HandleUtility.WorldToGUIPoint(new(
            //             tilePosition.x + tile.Size.x * 0.5f,
            //             tilePosition.y,
            //             tilePosition.z + tile.Size.z * 0.5f));
            //
            //         // Scene 뷰의 GUI 영역 그리기 시작
            //         Handles.BeginGUI();
            //         // GUI 영역을 설정 (오브젝트 위치에 따라 GUI 좌표를 정확히 설정)
            //
            //         string properties = string.Join(", \n   ", tile.Properties);
            //
            //         GUI.Label(new(guiPosition.x - 50, guiPosition.y - 25, 50, 30), $"TilePoint : {tile.TilePoint} \n [{properties}]", _style);
            //
            //         // GUI 그리기 종료
            //         Handles.EndGUI();
            //     }
            // }
        }
    }
}
#endif