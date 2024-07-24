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
    internal sealed class ComponentDropdown : TreeView
    {
        private readonly Action<GameObject> _onSelected;
        private readonly GameObject _rootObject;
        private readonly Type _filterType;
        private readonly Dictionary<int, GameObject> _itemsMap = new();
        private readonly StringBuilder _stringBuilder = new();
        private readonly ComponentDropdownPopupContent _content;

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

        public ComponentDropdown(TreeViewState state, GameObject rootObject, Type filterType, Action<GameObject> onSelected) : base(state)
        {
            _rootObject = rootObject;
            _filterType = filterType;
            _onSelected = onSelected;
            _content = new ComponentDropdownPopupContent(this);
            Reload();
        }

        public void Show(Rect rect)
        {
            PopupWindow.Show(rect, _content);
        }

        protected override TreeViewItem BuildRoot()
        {
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

                static void setIcon(TreeViewItem item, Type filterType, Dictionary<int, GameObject> itemsMap)
                {
                    Component component = itemsMap.TryGetValue(item.id, out GameObject go) ? go.GetComponent(filterType) : null;

                    item.icon = component ?
                        EditorGUIUtility.ObjectContent(component, filterType).image as Texture2D :
                        EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D;

                    if (item.hasChildren)
                    {
                        foreach (TreeViewItem treeViewItem in item.children)
                        {
                            setIcon(treeViewItem, filterType, itemsMap);
                        }
                    }
                }

                root.AddChild(rootItem);
                setIcon(rootItem, _filterType, _itemsMap);
            }

            return root;
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
            gameObjectItem.icon = EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D;

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

        private class ComponentDropdownPopupContent : PopupWindowContent
        {
            private readonly ComponentDropdown _dropdown;

            public ComponentDropdownPopupContent(ComponentDropdown dropdown)
            {
                _dropdown = dropdown;
            }

            public override Vector2 GetWindowSize()
            {
                return new Vector2(250, _dropdown.rowHeight * _dropdown.ItemCount);
            }

            public override void OnGUI(Rect rect)
            {
                _dropdown.OnGUI(rect);
            }
        }
    }
}
#endif