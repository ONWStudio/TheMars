#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Helpers;
using Onw.Extensions;
using static Onw.Editor.EditorGUIHelper;

namespace TMGUITool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        private int _selectedTab = 0;
        private string[] _tabs = null;

        private readonly List<IGUIDrawer> _guiDrawers = new(ReflectionHelper.GetChildClassesFromType<IGUIDrawer>());

        [MenuItem("Onw Studio/TM Tool GUI Drawer")]
        internal static void OnWindow()
        {
            GetWindow<GUIToolDrawer>().Show();
        }

        private void OnEnable()
        {
            _guiDrawers.ForEach(guiDrawer => guiDrawer.Awake());

            _tabs = _guiDrawers
                .Select(drawer => drawer.GetType().Name)
                .ToArray();

            (_guiDrawers[_selectedTab] as ILoadable)?.LoadDataFromLocal();
            _guiDrawers[_selectedTab].OnEnable();
        }

        private void OnGUI()
        {
            if (_guiDrawers.Count <= 0) return;

            int selectedTab = GUILayout.Toolbar(_selectedTab, _tabs);

            if (selectedTab != _selectedTab)
            {
                _selectedTab = selectedTab;
                _guiDrawers[_selectedTab].OnEnable();
            }

            if (_guiDrawers[_selectedTab] is ILoadable loader &&
                GUILayout.Button("데이터 저장 (로컬)"))
            {
                loader.SaveDataToLocal();
            }

            EditorGUILayout.Space();

            ActionHorizontal(() =>
            {
                if (_guiDrawers[_selectedTab] is not IPagable pager) return;

                pager.Page = EditorGUILayout.IntField(
                    $"Page {pager.MaxPage} / {pager.Page}",
                    pager.Page);

                System.Action pagingCallback = null;
                IGUIPagingHandler<IPagable> handler = pager as IGUIPagingHandler<IPagable>;

                if (GUILayout.Button("<<"))
                {
                    pager.Page = 1;
                    pagingCallback = handler is not null ? handler.OnFirst : null;
                }

                if (GUILayout.Button("<"))
                {
                    pager.Page--;
                    pagingCallback = handler is not null ? handler.OnLeft : null;
                }

                if (GUILayout.Button(">"))
                {
                    pager.Page++;
                    pagingCallback = handler is not null ? handler.OnRight : null;
                }

                if (GUILayout.Button(">>"))
                {
                    pager.Page = pager.MaxPage;
                    pagingCallback = handler is not null ? handler.OnLast : null;
                }

                pager.Page = Mathf.Clamp(
                    pager.Page,
                    pager.MaxPage > 0 ? 1 : 0,
                    pager.MaxPage);

                pagingCallback?.Invoke();

                if (pager is IMovedPage<IPagable> pageMoveHandler && pager.PrevPage != pager.Page)
                {
                    pageMoveHandler.OnMove();
                }

                pager.PrevPage = pager.Page;
            });

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

        private void OnDisable()
        {
            _guiDrawers
                .OfType<CustomInspectorEditorWindow>()
                .ForEach(customInspectorWindow => customInspectorWindow.OnDisable());
        }
    }
}
#endif