#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Onw.Editor.Window;
using Onw.Editor.Extensions;
using Onw.ScriptableObjects.Editor;
using TM.Card;

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
                    borderBottomColor = Color.black,
                    borderBottomWidth = 1f,
                    height = 25f,
                    maxHeight = 25f
                },
            };

            Button addButton = createHeaderButton(()
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMCardData>("Assets/OnwStudio/ScriptableObject/Cards", $"Card_No.{Guid.NewGuid()}")));
            addButton.text = "추가";
            addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#1B5914", out Color addColor) ? addColor : Color.green;
            addButton.SetChangedColorButtonEvent();
            header.Add(addButton);
            
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
                        height = 20f,
                        maxHeight = 20f,
                        minHeight = 20f
                    },
                };

                return button;
            }
        }

        protected override ScriptableObjectButton CreateButton(ScriptableObject so)
        {
            ScriptableObjectButton button = base.CreateButton(so);
            TMCardData card = (button.ScriptableObject as TMCardData)!;
            button.text = "";
            string cardName = card.GetLocalizedCardNameByLocale("ko-KR");
            Label field = new(cardName)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };
            button.Add(field);

            Button removeButton = new(() =>
            {
                if (_selectedObject?.ScriptableObject == so)
                {
                    _selectedObject = null;
                }
                
                RemoveSo(so);
                UnityEngine.Object.DestroyImmediate(so, true);
            })
            {
                style =
                {
                    position = Position.Absolute,
                    right = new Length(2, LengthUnit.Percent),
                    top = new Length(50, LengthUnit.Percent),                    
                    translate = new Translate(0, Length.Percent(-50)),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = ColorUtility.TryParseHtmlString("#4C1212", out Color removeColor) ? removeColor : Color.red,
                    height = 20,
                    width = 20,
                },
                text = "X",
            };
            
            removeButton.SetChangedColorButtonEvent();
            button.Add(removeButton);
            return button;
        }
    }
}
#endif