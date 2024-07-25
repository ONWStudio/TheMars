#if UNITY_EDITOR
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Onw.Attribute.Editor
{
    internal sealed class ComponentDropdown : TreeView
    {
        private class ComponentDropdownPopupContent : PopupWindowContent
        {
            private readonly ComponentDropdown _dropdown;
            private readonly SearchField _searchField;
            private readonly GUIStyle _labelStyle;

            public ComponentDropdownPopupContent(ComponentDropdown dropdown)
            {
                _dropdown = dropdown;
                _labelStyle = new(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter
                };
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

                EditorGUI.LabelField(
                    new(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 1.35f, rect.width, EditorGUIUtility.singleLineHeight),
                    "Nested Object",
                    _labelStyle);

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
        private readonly Type _defaultType;
        private readonly Type _filterType;
        private readonly Texture2D _gameObjectImage;
        private readonly Dictionary<int, GameObject> _itemsMap = new();
        private readonly Dictionary<int, GUIContent> _itemContentMap = new();
        private readonly StringBuilder _stringBuilder = new();
        private readonly ComponentDropdownPopupContent _content;
        private Texture2D _targetTexture = null;

        public ComponentDropdown(TreeViewState state, GameObject rootObject, Type filterType, Action<GameObject> onSelected) : base(state)
        {
            _rootObject = rootObject;
            _filterType = filterType;
            _onSelected = onSelected;
            rowHeight *= 1.5f;
            _defaultType = typeof(GameObject);
            _gameObjectImage = EditorGUIUtility.ObjectContent(null, _defaultType).image as Texture2D;
            _content = new(this);
        }

        protected override TreeViewItem BuildRoot()
        {
            _itemsMap.Clear();
            _itemContentMap.Clear();
            var root = new TreeViewItem(0, -1, "root");

            if (_filterType == typeof(GameObject))
            {
                addGameObjectToDropdown(root, _rootObject);
            }
            else
            {
                var rootItem = createItem(_rootObject, 0);

                foreach (Component component in _rootObject.GetComponentsInChildren(_filterType))
                {
                    _itemsMap.Add(component.gameObject.GetHashCode(), component.gameObject);
                    addComponentToDropdown(component.gameObject, new() { { _rootObject, rootItem } });
                }

                setDepth(rootItem);

                static void setDepth(TreeViewItem item, int depth = 0)
                {
                    item.depth = depth;

                    if (item.hasChildren)
                    {
                        foreach (TreeViewItem treeViewItem in item.children)
                        {
                            setDepth(treeViewItem, depth + 1);
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

            Rect labelRect = new(
                args.rowRect.x + (string.IsNullOrEmpty(searchString) ? EditorGUIUtility.singleLineHeight + (args.item.depth * EditorGUIUtility.singleLineHeight) : 0),
                args.rowRect.y + (args.rowRect.height - EditorGUIUtility.singleLineHeight) * 0.5f, 
                args.rowRect.width,
                EditorGUIUtility.singleLineHeight);

            if (!_itemContentMap.TryGetValue(args.item.id, out GUIContent guiContent))
            {
                guiContent = new GUIContent(item.displayName, getIconForItem(item));
            }

            EditorGUI.LabelField(labelRect, guiContent);
        }

        protected override void DoubleClickedItem(int id)
        {
            if (!_itemsMap.TryGetValue(id, out GameObject @object)) return;

            _content.editorWindow.Close();
            _onSelected?.Invoke(@object);
        }

        public void Show(Rect rect)
        {
            Reload();
            ExpandAll();
            PopupWindow.Show(rect, _content);
        }

        private Texture2D getIconForItem(TreeViewItem item)
        {
            Texture2D getFilterTypeToTexture(Component component)
            {
                if (!_targetTexture)
                {
                    _targetTexture = EditorGUIUtility.ObjectContent(component, _filterType).image as Texture2D;
                }

                return _targetTexture;
            }

            return _filterType == _defaultType ||
                   !_itemsMap.TryGetValue(item.id, out var gameObject) ||
                   !gameObject.TryGetComponent(_filterType, out Component component) ?
                _gameObjectImage :
                getFilterTypeToTexture(component);
        }

        private void addComponentToDropdown(GameObject selectedObject, Dictionary<GameObject, TreeViewItem> visited, TreeViewItem prevItem = null)
        {
            if (visited.TryGetValue(selectedObject, out TreeViewItem advancedDropdownItem))
            {
                if (prevItem is not null)
                {
                    advancedDropdownItem.AddChild(prevItem);
                }

                return;
            }

            var gameObjectItem = createItem(selectedObject, 0);
            if (prevItem is not null)
            {
                gameObjectItem.AddChild(prevItem);
            }

            addComponentToDropdown(selectedObject.transform.parent.gameObject, visited, gameObjectItem);
        }

        private void addGameObjectToDropdown(TreeViewItem parent, GameObject gameObject, int depth = 0)
        {
            var gameObjectItem = createItem(gameObject, depth);

            parent.AddChild(gameObjectItem);
            _itemsMap.Add(gameObjectItem.id, gameObject);

            foreach (Transform child in gameObject.transform)
            {
                addGameObjectToDropdown(gameObjectItem, child.gameObject, depth + 1);
            }
        }

        private TreeViewItem createItem(GameObject gameObject, int depth)
        {
            return new TreeViewItem(gameObject.GetHashCode(), depth, buildString(gameObject));
        }

        private string buildString(GameObject go)
        {
            _stringBuilder.Append(go.name);
            _stringBuilder.Append(" (");
            _stringBuilder.Append(_filterType == _defaultType || go.TryGetComponent(_filterType, out Component _) ? _filterType.Name : _defaultType.Name);
            _stringBuilder.Append(")");
            string name = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return name;
        }
    }
}
#endif
