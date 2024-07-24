#if UNITY_EDITOR
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Onw.Editor.Attribute
{
    using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;
    using GUI = UnityEngine.GUI;

    internal sealed class ComponentDropdown : TreeView
    {
        private class ComponentDropdownPopupContent : PopupWindowContent
        {
            private readonly ComponentDropdown _dropdown;
            private readonly SearchField _searchField;

            public ComponentDropdownPopupContent(ComponentDropdown dropdown)
            {
                _dropdown = dropdown;
                _searchField = new SearchField();
            }

            public override Vector2 GetWindowSize()
            {
                return new Vector2(250f, Mathf.Clamp(_dropdown.ItemCount * _dropdown.rowHeight, 200, 500));
            }

            public override void OnGUI(Rect rect)
            {
                float voidWidth = rect.width * 0.1f;
                rect.y += 3.0f;
                Rect searchRect = new(rect.x + voidWidth * 0.5f, rect.y, rect.width - voidWidth, EditorGUIUtility.singleLineHeight);

                _dropdown.searchString = _searchField.OnGUI(searchRect, _dropdown.searchString);

                GUIStyle guiStyle = new(EditorStyles.boldLabel);
                guiStyle.alignment = TextAnchor.MiddleCenter;

                EditorGUI.LabelField(
                    new(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 1.35f, rect.width, EditorGUIUtility.singleLineHeight),
                    "Nested Object",
                    guiStyle);

                Rect treeViewRect = new(
                    rect.x, 
                    rect.y + EditorGUIUtility.singleLineHeight * 2.5f, 
                    rect.width,
                    rect.height - EditorGUIUtility.singleLineHeight);

                _dropdown.OnGUI(treeViewRect);
            }
        }

        public int ItemCount
        {
            get
            {
                static int countItems(TreeViewItem item)
                {
                    return item.children?.Aggregate(1, (count, child) => count + countItems(child)) ?? 1;
                }

                return countItems(rootItem);
            }
        }

        private readonly Action<GameObject> _onSelected;
        private readonly GameObject _rootObject;
        private readonly Type _filterType;
        private readonly Dictionary<int, GameObject> _itemsMap = new();
        private readonly StringBuilder _stringBuilder = new();
        private readonly ComponentDropdownPopupContent _content;

        public ComponentDropdown(TreeViewState state, GameObject rootObject, Type filterType, Action<GameObject> onSelected) : base(state)
        {
            _rootObject = rootObject;
            _filterType = filterType;
            _onSelected = onSelected;
            rowHeight *= 1.5f;
            _content = new(this);
        }

        public void Show(Rect rect)
        {
            Reload();
            ExpandAll();
            PopupWindow.Show(rect, _content);
        }

        protected override TreeViewItem BuildRoot()
        {
            _itemsMap.Clear();
            var root = new TreeViewItem(0, -1, "root");

            if (_filterType == typeof(GameObject))
            {
                AddGameObjectToDropdown(root, _rootObject);
            }
            else
            {
                var rootItem = CreateItem(_rootObject, 0);

                foreach (Component component in _rootObject.GetComponentsInChildren(_filterType))
                {
                    _itemsMap.Add(component.gameObject.GetHashCode(), component.gameObject);
                    AddComponentToDropdown(component.gameObject, new() { { _rootObject, rootItem } });
                }

                SetDepth(rootItem);

                static void SetDepth(TreeViewItem item, int depth = 0)
                {
                    item.depth = depth;

                    if (item.hasChildren)
                    {
                        foreach (TreeViewItem treeViewItem in item.children)
                        {
                            SetDepth(treeViewItem, depth + 1);
                        }
                    }
                }

                root.AddChild(rootItem);
            }

            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            var rows = base.BuildRows(root);

            if (!string.IsNullOrEmpty(searchString))
            {
                rows = rows.Where(item => item.displayName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            return rows;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item;
            // Calculate the rect for the label to be vertically centered
            Rect labelRect = new(
                args.rowRect.x + 15 + (args.item.depth * 15),
                args.rowRect.y + (args.rowRect.height - EditorGUIUtility.singleLineHeight) / 2, 
                args.rowRect.width,
                EditorGUIUtility.singleLineHeight);

            var icon = GetIconForItem(item);
            var content = new GUIContent(item.displayName, icon);

            EditorGUI.LabelField(labelRect, content);
        }

        private Texture2D GetIconForItem(TreeViewItem item)
        {
            return _filterType == typeof(GameObject) ||
                   !_itemsMap.TryGetValue(item.id, out var gameObject) ||
                   !gameObject.TryGetComponent(_filterType, out Component component) ? 
                EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D :
                EditorGUIUtility.ObjectContent(component, _filterType).image as Texture2D;
        }

        protected override void DoubleClickedItem(int id)
        {
            if (!_itemsMap.TryGetValue(id, out GameObject @object)) return;

            _content.editorWindow.Close();
            _onSelected?.Invoke(@object);
        }

        private void AddComponentToDropdown(GameObject selectedObject, Dictionary<GameObject, TreeViewItem> visited, TreeViewItem prevItem = null)
        {
            if (visited.TryGetValue(selectedObject, out TreeViewItem advancedDropdownItem))
            {
                if (prevItem is not null)
                {
                    advancedDropdownItem.AddChild(prevItem);
                }

                return;
            }

            var gameObjectItem = CreateItem(selectedObject, 0);
            if (prevItem is not null)
            {
                gameObjectItem.AddChild(prevItem);
            }

            AddComponentToDropdown(selectedObject.transform.parent.gameObject, visited, gameObjectItem);
        }

        private void AddGameObjectToDropdown(TreeViewItem parent, GameObject gameObject, int depth = 0)
        {
            var gameObjectItem = CreateItem(gameObject, depth);

            parent.AddChild(gameObjectItem);
            _itemsMap.Add(gameObjectItem.id, gameObject);

            foreach (Transform child in gameObject.transform)
            {
                AddGameObjectToDropdown(gameObjectItem, child.gameObject, depth + 1);
            }
        }

        private TreeViewItem CreateItem(GameObject gameObject, int depth)
        {
            return new TreeViewItem(gameObject.GetHashCode(), depth, BuildString(gameObject));
        }

        private string BuildString(GameObject go)
        {
            _stringBuilder.Append(go.name);
            _stringBuilder.Append(" (");
            _stringBuilder.Append(_filterType == typeof(GameObject) || go.TryGetComponent(_filterType, out Component _) ? _filterType.Name : typeof(GameObject).Name);
            _stringBuilder.Append(")");
            string name = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return name;
        }
    }
}
#endif
