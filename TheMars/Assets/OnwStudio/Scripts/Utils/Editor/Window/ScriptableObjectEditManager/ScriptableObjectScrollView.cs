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

        public event Action<ScriptableObject> OnSelectObject;

        public IReadOnlyList<ScriptableObject> ScriptableObjects => _scriptableObjects;
        protected readonly List<ScriptableObject> _scriptableObjects = new();

        internal void Initialize()
        {
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
            ScriptableObject[] soArray = objects.ToArray();
            soArray
                .Select(CreateButton)
                .ForEach(View.Add);
            
            _scriptableObjects.AddRange(soArray);
        }

        public void AddSo(ScriptableObject obj)
        {
            Button button = CreateButton(obj);
            View.Add(button);
            _scriptableObjects.Add(obj);
        }

        protected virtual Button CreateButton(ScriptableObject so) => new(() => OnSelectObject?.Invoke(so))
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                unityTextAlign = TextAnchor.MiddleCenter,
                flexGrow = 1,
                height= 40,
            },
            text = so.name
        };

        protected virtual VisualElement CreateHeader()
        {
            return null;
        }
    }
}
#endif
