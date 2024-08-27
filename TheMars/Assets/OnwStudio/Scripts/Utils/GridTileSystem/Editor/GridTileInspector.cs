#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Helper;
using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;
using UnityEngine.UIElements;

namespace Onw.GridTile.Editor
{
    using Editor = UnityEditor.Editor;
    using Event = UnityEngine.Event;

    [CustomEditor(typeof(GridTile))]
    internal sealed class GridTileInspector : Editor
    {
        private const string WINDOW_OPTION_KEY = "GridManagerInspector_WindowOption";

        private Rect _windowRect = new(40, 30, 200, 400); // 창의 초기 위치와 크기
        private bool _isResizing = false;                      // 크기 조절 상태 확인
        private bool _isDragging = false;                      // 드래그 상태 확인

        private GridTile _gridTile = null;
        private GridManager _gridManager = null;
        private GUIStyle _windowStyle = null;

        private void OnEnable()
        {
            _gridTile = target as GridTile;
            _gridManager = _gridTile!
                .GetType()
                .GetField("_gridManager", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(_gridTile) as GridManager;

            if (EditorPrefs.HasKey(WINDOW_OPTION_KEY))
            {
                SerializedRect windowOption = JsonUtility.FromJson<SerializedRect>(EditorPrefs.GetString(WINDOW_OPTION_KEY));
                //_windowRect = windowOption.ToRect();
            }
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(WINDOW_OPTION_KEY, JsonUtility.ToJson(new SerializedRect(_windowRect)));
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Select Manager"))
            {
                Selection.activeGameObject = _gridManager.gameObject;
            }
        }

        private void OnSceneGUI()
        {
            Handles.BeginGUI();

            if (_windowStyle is null)
            {
                _windowStyle = new(GUI.skin.window);
                _windowStyle.active = _windowStyle.normal;
                _windowStyle.focused = _windowStyle.normal;
                _windowStyle.onNormal = _windowStyle.normal;
                _windowStyle.onActive = _windowStyle.normal;
                _windowStyle.onFocused = _windowStyle.normal;
                _windowStyle.stretchWidth = true;
                _windowStyle.stretchHeight = true;
            }

            _windowRect = GUI.Window(0, _windowRect, drawWindowContents, "Tile Option", _windowStyle);

            handleWindowDragAndResize();
            Handles.EndGUI();
        }

        private void drawWindowContents(int windowID)
        {
            // Inspector 내용 그리기
            DrawDefaultInspector();

            if (GUILayout.Button("Select Manager"))
            {
                Selection.activeGameObject = _gridManager.gameObject;
            }

            GUI.DragWindow(new(0, 0, _windowRect.width, 20)); // 상단 20px 영역을 드래그 가능하도록 설정
        }

        private void handleWindowDragAndResize()
        {
            Event currentEvent = Event.current;
            Vector2 mousePosition = currentEvent.mousePosition;

            Rect leftTopArea = new(_windowRect.xMin - 5, _windowRect.yMin - 5, 10, 10);
            Rect leftArea = new(_windowRect.xMin - 5, _windowRect.yMin + 5, 10, _windowRect.height - 10);
            Rect leftBottomArea = new(_windowRect.xMin - 5, _windowRect.yMax - 5, 10, 10);
            Rect bottomArea = new(_windowRect.xMin + 5, _windowRect.yMax - 5, _windowRect.width - 10, 10);
            Rect rightBottomArea = new(_windowRect.xMax - 5, _windowRect.yMax - 5, 10, 10);
            Rect rightArea = new(_windowRect.xMax - 5, _windowRect.yMin + 5, 10, _windowRect.height - 10);
            Rect topArea = new(_windowRect.xMin + 5, _windowRect.yMin - 5, _windowRect.width - 10, 10);
            Rect rightTopArea = new(_windowRect.xMax - 5, _windowRect.yMin - 5, 10, 10);

            setCursor(leftTopArea, MouseCursor.ResizeUpLeft);
            setCursor(leftArea, MouseCursor.ResizeHorizontal);
            setCursor(leftBottomArea, MouseCursor.ResizeUpRight);
            setCursor(bottomArea, MouseCursor.ResizeVertical);
            setCursor(rightBottomArea, MouseCursor.ResizeUpLeft);
            setCursor(rightArea, MouseCursor.ResizeHorizontal);
            setCursor(topArea, MouseCursor.ResizeVertical);
            setCursor(rightTopArea, MouseCursor.ResizeUpRight);

            if (currentEvent.type == EventType.MouseDown)
            {

            }

            void setCursor(Rect rect, MouseCursor cursor)
            {
                EditorGUIUtility.AddCursorRect(SceneView.currentDrawingSceneView.rootVisualElement.parent.parent.contentRect, MouseCursor.ResizeUpRight);
            }

        }
    }
}
#endif