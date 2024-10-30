#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Onw.Editor.Window;
using Onw.ScriptableObjects.Editor;
using TM.Card;
using Onw.Editor.Extensions;

namespace TM.Editor
{
    internal sealed class TMCardScrollView : ScriptableObjectScrollView
    {
        protected override VisualElement CreateHeader()
        {
            VisualElement header = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1,
                    borderBottomColor = Color.black,
                    borderBottomWidth = 1,
                    paddingTop = 2,
                    paddingBottom = 10,
                    minHeight = 50,
                },
            };

            Button addButton = createHeaderButton(()
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMCardData>("Assets/OnwStudio/ScriptableObject/Cards", $"Card_No.{Guid.NewGuid()}")));
            addButton.text = "추가";
            addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#1B5914", out Color addColor) ? addColor : Color.green;
            addButton.SetChangedColorButtonEvent();

            Button removeButton = createHeaderButton(() => 
            {
                ScriptableObject scriptableObject = _selectedObject?.ScriptableObject;
                RemoveSo(scriptableObject);
                _selectedObject = null;
                UnityEngine.Object.DestroyImmediate(scriptableObject, true);
            });
            removeButton.text = "제거";
            removeButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#4C1212", out Color removeColor) ? removeColor : Color.red;
            removeButton.SetChangedColorButtonEvent();

            header.Add(addButton);
            header.Add(removeButton);

            return header;

            static Button createHeaderButton(Action clickedEvent)
            {
                Button button = new(clickedEvent)
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        flexGrow = 1,
                        height = 20
                    },
                };

                button.clicked += clickedEvent;

                return button;
            }
        }
    }
}
#endif
