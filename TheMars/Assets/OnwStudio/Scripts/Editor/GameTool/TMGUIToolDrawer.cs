#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorTool.EditorTool;

namespace TMGUITool
{
    internal sealed partial class TMGUIToolDrawer : EditorWindow
    {
        private int _prevPage = 0;
        private int _selectedTab = 0;
        private string[] _tabs = null;

        private readonly List<IGUIDrawer> _guiDrawers = new(ReflectionHelper.GetChildClassesFromType<IGUIDrawer>());

        [MenuItem("Onw Studio/TM Tool GUI Drawer")]
        internal static void OnWindow()
        {
            GetWindow<TMGUIToolDrawer>().Show();
        }

        private void OnEnable()
        {
            _guiDrawers.ForEach(guiDrawer => guiDrawer.Awake());

            _tabs = _guiDrawers
                .Select(drawer => drawer.GetType().Name)
                .ToArray();

            _guiDrawers[_selectedTab].LoadDataFromLocal();
            _guiDrawers[_selectedTab].OnEnable();
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "카테고리를 변경하거나 창을 종료 시 저장이 되지 않습니다 카테고리 변경전에 로컬에 저장해주세요",
                MessageType.Info,
                true);

            if (_guiDrawers.Count <= 0)
            {
                return;
            }

            int selectedTab = GUILayout.Toolbar(_selectedTab, _tabs);

            if (selectedTab != _selectedTab)
            {
                _selectedTab = selectedTab;

                _guiDrawers[_selectedTab].OnEnable();
            }

            if (GUILayout.Button("데이터 저장 (로컬)"))
            {
                _guiDrawers[_selectedTab].SaveDataToLocal();
            }

            EditorGUILayout.Space();

            ActionHorizontal(() =>
            {
                _guiDrawers[_selectedTab].Page = EditorGUILayout.IntField(
                    $"Page {_guiDrawers[_selectedTab].MaxPage} / {_guiDrawers[_selectedTab].Page}",
                    _guiDrawers[_selectedTab].Page);

                IGUIPagingHandler pagingHandler = _guiDrawers[_selectedTab] as IGUIPagingHandler;
                System.Action pagingCallback = null;

                if (GUILayout.Button("<<"))
                {
                    _guiDrawers[_selectedTab].Page = 1;
                    pagingCallback = () => pagingHandler?.OnFirst();
                }

                if (GUILayout.Button("<"))
                {
                    _guiDrawers[_selectedTab].Page--;
                    pagingCallback = () => pagingHandler?.OnLeft();
                }

                if (GUILayout.Button(">"))
                {
                    _guiDrawers[_selectedTab].Page++;
                    pagingCallback = () => pagingHandler?.OnRight();
                }

                if (GUILayout.Button(">>"))
                {
                    _guiDrawers[_selectedTab].Page = _guiDrawers[_selectedTab].MaxPage;
                    pagingCallback = () => pagingHandler?.OnLast();
                }

                _guiDrawers[_selectedTab].Page = Mathf.Clamp(
                    _guiDrawers[_selectedTab].Page,
                    _guiDrawers[_selectedTab].MaxPage == 0 ? 0 : 1,
                    _guiDrawers[_selectedTab].MaxPage);

                pagingCallback?.Invoke();
            });

            IMovedPage movedPage = _guiDrawers[_selectedTab] as IMovedPage;

            if (movedPage is not null && _prevPage != _guiDrawers[_selectedTab].Page)
            {
                movedPage.OnMove();
            }

            _prevPage = _guiDrawers[_selectedTab].Page;

            ActionEditorVertical(() => _guiDrawers[_selectedTab].OnDraw(), GUI.skin.box);

            if (_guiDrawers[_selectedTab].HasErrors)
            {
                ActionEditorHorizontal(() =>
                {
                    EditorGUILayout.HelpBox("유효하지 않은 데이터가 있습니다!", MessageType.Error);

                    if (GUILayout.Button("OK"))
                    {
                        _guiDrawers[_selectedTab].HasErrors = false;
                    }
                });
            }

            if (_guiDrawers[_selectedTab].IsSuccess ||
                !string.IsNullOrEmpty(_guiDrawers[_selectedTab].Message))
            {
                ActionEditorHorizontal(() =>
                {
                    EditorGUILayout.HelpBox(
                        _guiDrawers[_selectedTab].Message,
                        _guiDrawers[_selectedTab].IsSuccess ? MessageType.Info : MessageType.Error);

                    if (GUILayout.Button("확인"))
                    {
                        _guiDrawers[_selectedTab].IsSuccess = false;
                        _guiDrawers[_selectedTab].Message = string.Empty;
                    }
                });
            }
        }
    }
}
#endif