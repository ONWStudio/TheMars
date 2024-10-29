#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Onw.Editor.Window
{
    internal class ScriptableObjectScrollView : VisualElement
    {
        public ScrollView View { get; } = new()
        {
            style =
            {
                flexDirection = FlexDirection.Column,
                flexGrow = 1

            },
            mode = ScrollViewMode.Vertical
        };

        public event Action<ScriptableObjectButton> OnSelectObject;

        public IReadOnlyList<ScriptableObjectButton> ScriptableObjects => _scriptableObjects;
        protected readonly List<ScriptableObjectButton> _scriptableObjects = new();

        protected ScriptableObjectButton _selectedObject = null;

        internal void Initialize()
        {
            OnSelectObject += selectedObject => _selectedObject = selectedObject;
            VisualElement header = CreateHeader();
            if (header is not null)
            {
                Add(header);
            }
            Add(View);
        }
        
        internal void ClearList()
        {
            View.Clear();
            _scriptableObjects.Clear();
        }

        public void AddRange(IEnumerable<ScriptableObject> objects)
        {
            ScriptableObjectButton[] soArray = objects
                .Select(CreateButton)
                .ToArray();

            soArray.ForEach(View.Add);
            _scriptableObjects.AddRange(soArray);
        }

        public void AddSo(ScriptableObject obj)
        {
            ScriptableObjectButton button = CreateButton(obj);
            View.Add(button);
            _scriptableObjects.Add(button);
        }

        public void RemoveSo(ScriptableObject obj)
        {
            int index = _scriptableObjects.FindIndex(button => button.ScriptableObject == obj);

            if (index > -1)
            {
                ScriptableObjectButton soButton = _scriptableObjects[index];
                _scriptableObjects.RemoveAt(index);
                soButton.RemoveFromHierarchy();
            }
        }

        protected virtual ScriptableObjectButton CreateButton(ScriptableObject so)
        {
            ScriptableObjectButton button = new(so)
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                    height= 40,
                },
                text = so.name,
            };
            button.clicked += () => OnSelectObject?.Invoke(button);

            return button;
        }

        protected virtual VisualElement CreateHeader()
        {
            return null;
        }

        public ScriptableObjectScrollView()
        {
            style.borderBottomColor = Color.black;
            style.borderTopColor = Color.black;
            style.borderLeftColor = Color.black;
            style.borderRightColor = Color.black;
            style.borderBottomWidth = 1;
            style.borderTopWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
        }
    }
}
#endif
