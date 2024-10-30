#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.ScriptableObjects.Editor;
using TM.Building;
using TM.Card;
using TM.Event;
using TM.Synergy;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Onw.Editor.Window
{
    internal readonly struct ScrollBuildOption
    {
        public Type ScriptableObjectType { get; }
        public Type ScrollViewType { get; }
        public string Label { get; }

        public ScrollBuildOption(Type scriptableObjectType, string label)
        {
            ScriptableObjectType = scriptableObjectType;
            ScrollViewType = null;
            Label = label;
        }

        public ScrollBuildOption(Type scriptableObjectType, Type scrollViewType, string label) : this(scriptableObjectType, label)
        {
            ScrollViewType = scrollViewType;
        }
    }

    internal abstract class ScriptableObjectEditManager : EditorWindow
    {
        public event Action<string> OnChangedSearchString;

        public string SeartchString 
        {
            get => _searchString;
            set
            {
                _searchString = value;
                OnChangedSearchString?.Invoke(_searchString);
            }
        }

        private ToolbarButton _selectedButton = null;
        private Color _originalColor = ColorUtility.TryParseHtmlString("#505050", out Color color) ? color : Color.grey;

        private string _searchString = string.Empty;

        protected abstract ScrollBuildOption[] GetTargetScriptableObjectType();

        protected void CreateGUI()
        {
            Debug.Log("Create GUI!");

            VisualElement root = rootVisualElement;
            Toolbar toolbar = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    backgroundColor = ColorUtility.TryParseHtmlString("#353535", out Color bgColor) ? bgColor : Color.gray,
                    paddingLeft = 1,
                    paddingRight = 1,
                    paddingTop = 1,
                    paddingBottom = 1,
                    height = 36,
                    maxHeight = 36,
                    flexGrow = 1,
                    borderBottomColor = Color.white
                }
            };

            VisualElement contentElement = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1,
                    flexShrink = 1,
                    borderBottomColor = Color.black,
                    borderTopColor = Color.black,
                    borderLeftColor = Color.black,
                    borderRightColor = Color.black,
                    borderBottomWidth = 1,
                    borderTopWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                }
            };


            foreach ((ToolbarButton, ScriptableObjectScrollView) buttonOption in GetTargetScriptableObjectType()
                .Where(option => option.ScriptableObjectType.IsSubclassOf(typeof(ScriptableObject)))
                .Select(getButtonOption))
            {
                setToolbarButton(buttonOption.Item1);
                toolbar.Add(buttonOption.Item1);
                buttonOption.Item2.name = "SOScrollView";
                buttonOption.Item1.clicked += () =>
                {
                    VisualElement scrollView = root.Q<ScriptableObjectScrollView>("SOScrollView");
                    scrollView?.RemoveFromHierarchy();

                    contentElement.Add(buttonOption.Item2);
                };
            }

            root.Add(toolbar);
            root.Add(contentElement);

            static (ToolbarButton, ScriptableObjectScrollView) getButtonOption(ScrollBuildOption buildOption)
            {
                (ToolbarButton, ScriptableObjectScrollView) buttonOption;

                buttonOption.Item1 = new()
                {
                    text = buildOption.Label
                };

                buttonOption.Item2 = buildOption.ScrollViewType is null || !buildOption.ScrollViewType.IsSubclassOf(typeof(ScriptableObjectScrollView)) ?
                    new() :
                    (Activator.CreateInstance(buildOption.ScrollViewType) as ScriptableObjectScrollView)!;

                buttonOption.Item2.Initialize();
                buttonOption.Item2.AddRange(ScriptableObjectHandler.LoadAllScriptableObjects(buildOption.ScriptableObjectType));

                return buttonOption;
            }

            void setToolbarButton(ToolbarButton button)
            {
                button.style.backgroundColor = _originalColor;
                button.style.unityTextAlign = TextAnchor.MiddleCenter;
                button.style.flexGrow = 1;
                button.style.marginRight = 0.25f;
                button.style.marginLeft = 0.25f;
                button.style.height = 30;
                button.clicked += () =>
                {
                    if (_selectedButton is not null)
                    {
                        _selectedButton.style.backgroundColor = _originalColor;
                    }

                    _selectedButton = button;
                    if (ColorUtility.TryParseHtmlString("#44577B", out Color btnColor))
                    {
                        _selectedButton.style.backgroundColor = btnColor;
                    }
                };
            }
        }
    }
}
#endif