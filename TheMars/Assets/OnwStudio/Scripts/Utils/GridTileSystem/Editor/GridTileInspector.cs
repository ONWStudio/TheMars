#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Extensions;
using Onw.Editor;
using Onw.Editor.GUI;
using UnityEngine;
using UnityEditor;

namespace Onw.GridTile.Editor
{
    using Editor = UnityEditor.Editor;
    using Event = UnityEngine.Event;

    [CustomEditor(typeof(GridTile)), CanEditMultipleObjects]
    internal sealed class GridTileInspector : Editor
    {
        private GridTile _gridTile = null;
        private GridManager _gridManager = null;
        private readonly SceneViewInnerWindow<GridTileInspector> _sceneViewInnerWindow = new();

        private void OnEnable()
        {
            _gridTile = target as GridTile;
            _gridManager = _gridTile!
                .GetType()
                .GetField("_gridManager", BindingFlags.Instance | BindingFlags.NonPublic)?
                .GetValue(_gridTile) as GridManager;
            
            _sceneViewInnerWindow.OnEnable();
        }

        private void OnDisable()
        {
            _sceneViewInnerWindow.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        private void OnSceneGUI()
        {
            _sceneViewInnerWindow?.OnSceneGUI(drawWindowContents);
        }

        private void drawWindowContents(int windowID)
        {
            // Inspector 내용 그리기
            DrawDefaultInspector();

            if (GUILayout.Button("Select Manager"))
            {
                Selection.activeGameObject = _gridManager.gameObject;
            }
        }
    }
}
#endif