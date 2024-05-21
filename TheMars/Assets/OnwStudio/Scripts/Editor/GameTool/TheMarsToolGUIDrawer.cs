#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorTool.EditorTool;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
    {
        private int _selectedTab = 0;
        private string[] _tabs = null;

        private readonly List<IGUIDrawer> _playFabDataGUIDrawers = new();

        [MenuItem("Onw Studio/The Mars Tool GUI Drawer")]
        internal static void OnWindow()
        {
            GetWindow<TheMarsGUIToolDrawer>().Show();
        }

        private void OnEnable()
        {
            _playFabDataGUIDrawers.AddRange(ReflectionHelper.GetInterfacesFromType<IGUIDrawer>());

            _tabs = _playFabDataGUIDrawers
                .Select(drawer => drawer.GetType().Name)
                .ToArray();

            _playFabDataGUIDrawers[_selectedTab].LoadDataFromLocal();
            _playFabDataGUIDrawers[_selectedTab].OnEnable();
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "카테고리를 변경하거나 창을 종료 시 저장이 되지 않습니다 카테고리 변경전에 로컬에 저장해주세요",
                MessageType.Info,
                true);

            int selectedTab = GUILayout.Toolbar(_selectedTab, _tabs);

            if (selectedTab != _selectedTab)
            {
                _selectedTab = selectedTab;

                _playFabDataGUIDrawers[_selectedTab].LoadDataFromLocal();
                _playFabDataGUIDrawers[_selectedTab].OnEnable();
            }

            if (GUILayout.Button("데이터 저장 (로컬)"))
            {
                _playFabDataGUIDrawers[_selectedTab].SaveDataToLocal();
            }

            EditorGUILayout.Space();

            ActionHorizontal(() =>
            {
                _playFabDataGUIDrawers[_selectedTab].Page = EditorGUILayout.IntField(
                    $"Page {_playFabDataGUIDrawers[_selectedTab].MaxPage} / {_playFabDataGUIDrawers[_selectedTab].Page}",
                    _playFabDataGUIDrawers[_selectedTab].Page);

                IGUIPagingHandler pagingHandler = _playFabDataGUIDrawers[_selectedTab] as IGUIPagingHandler;
                System.Action pagingCallback = null;

                if (GUILayout.Button("<<"))
                {
                    _playFabDataGUIDrawers[_selectedTab].Page = 1;
                    pagingCallback = () => pagingHandler?.OnFirst();
                }

                if (GUILayout.Button("<"))
                {
                    _playFabDataGUIDrawers[_selectedTab].Page--;
                    pagingCallback = () => pagingHandler?.OnLeft();
                }

                if (GUILayout.Button(">"))
                {
                    _playFabDataGUIDrawers[_selectedTab].Page++;
                    pagingCallback = () => pagingHandler?.OnRight();
                }

                if (GUILayout.Button(">>"))
                {
                    _playFabDataGUIDrawers[_selectedTab].Page = _playFabDataGUIDrawers[_selectedTab].MaxPage;
                    pagingCallback = () => pagingHandler?.OnLast();
                }

                _playFabDataGUIDrawers[_selectedTab].Page = Mathf.Clamp(
                    _playFabDataGUIDrawers[_selectedTab].Page,
                    _playFabDataGUIDrawers[_selectedTab].MaxPage == 0 ? 0 : 1,
                    _playFabDataGUIDrawers[_selectedTab].MaxPage);

                pagingCallback?.Invoke();
            });

            ActionEditorVertical(() => _playFabDataGUIDrawers[_selectedTab].OnDraw(), GUI.skin.box);

            if (_playFabDataGUIDrawers[_selectedTab].HasErrors)
            {
                ActionEditorHorizontal(() =>
                {
                    EditorGUILayout.HelpBox("유효하지 않은 데이터가 있습니다!", MessageType.Error);

                    if (GUILayout.Button("OK"))
                    {
                        _playFabDataGUIDrawers[_selectedTab].HasErrors = false;
                    }
                });
            }

            if (_playFabDataGUIDrawers[_selectedTab].IsSuccess ||
                !string.IsNullOrEmpty(_playFabDataGUIDrawers[_selectedTab].Message))
            {
                ActionEditorHorizontal(() =>
                {
                    EditorGUILayout.HelpBox(
                        _playFabDataGUIDrawers[_selectedTab].Message,
                        _playFabDataGUIDrawers[_selectedTab].IsSuccess ? MessageType.Info : MessageType.Error);

                    if (GUILayout.Button("확인"))
                    {
                        _playFabDataGUIDrawers[_selectedTab].IsSuccess = false;
                        _playFabDataGUIDrawers[_selectedTab].Message = string.Empty;
                    }
                });
            }
        }
    }
}
#endif