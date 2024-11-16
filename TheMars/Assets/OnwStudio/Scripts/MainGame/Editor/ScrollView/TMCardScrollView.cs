#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using Onw.Editor.Window;
using Onw.Editor.Extensions;
using Onw.Extensions;
using Onw.Localization.Editor;
using Onw.ScriptableObjects.Editor;
using TM.Card;
using Unity.EditorCoroutines.Editor;

namespace TM.Editor
{
    internal sealed class TMCardScrollView : ScriptableObjectScrollView
    {
        private readonly List<EditorCoroutine> _cardNameObservers = new();
        
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

            Button creationButton = createHeaderButton(()
                => AddSo(ScriptableObjectHandler.CreateScriptableObject<TMCardData>("Assets/OnwStudio/ScriptableObject/Cards", $"Card_No.{Guid.NewGuid()}")));
            creationButton.text = "추가";
            creationButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#1B5914", out Color addColor) ? addColor : Color.green;
            creationButton.SetChangedColorButtonEvent();
            header.Add(creationButton);
            
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
            ScriptableObjectButton objectButton = base.CreateButton(so);
            TMCardData card = (objectButton.ScriptableObject as TMCardData)!;
            
            objectButton.text = "";
                
            Label field = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    flexGrow = 1,
                }
            };
            
            if (card.TryGetFieldByName(
                "_localizedCardName",
                BindingFlags.NonPublic | BindingFlags.Instance,
                out LocalizedString localizedCardName))
            {
                _cardNameObservers.Add(localizedCardName.MonitorSpecificLocaleEntry("ko-KR", onChangedString));
                if (localizedCardName.IsEmpty)
                {
                    objectButton.name = field.text = card.name;
                }
                else
                {
                    objectButton.name = field.text = localizedCardName.GetLocalizedString();
                }
                
                void onChangedString(string cardName)
                {
                    objectButton.name = field.text = string.IsNullOrEmpty(cardName) ? card.name : cardName;
                }
            }
            else
            {
                objectButton.name = field.text = card.name;
            }

            objectButton.Add(field);
            
            Button removeButton = new(() =>
            {
                if (_selectedObject?.ScriptableObject == so)
                {
                    _selectedObject = null;
                }
                
                RemoveSo(so);
                TMCardDataManager.Instance.RemoveCard(card);
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
            
            bool hasCard = TMCardDataManager.Instance.CardDataList.Contains(card);
            
            Button addButton = new()
            {
                style =
                {
                    position = Position.Absolute,
                    right = new Length(10, LengthUnit.Percent),
                    top = new Length(50, LengthUnit.Percent),                    
                    translate = new Translate(0, Length.Percent(-50)),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = ColorUtility.TryParseHtmlString(
                        hasCard ? "#4447A6" : "#2B2D6D", 
                        out Color addButtonColor) ? 
                        addButtonColor : 
                        Color.blue,
                    height = 20,
                    width = 20,
                },
                text = hasCard ? "On" : "Off",
            };

            addButton.clicked += () =>
            {
                bool hasCardByClicked = TMCardDataManager.Instance.CardDataList.Contains(card);

                if (hasCardByClicked)
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#2B2D6D", out Color color) ? color : Color.blue;
                    addButton.text = "Off";
                    TMCardDataManager.Instance.RemoveCard(card);
                }
                else
                {
                    addButton.style.backgroundColor = ColorUtility.TryParseHtmlString("#4447A6", out Color color) ? color : Color.blue;
                    addButton.text = "On";
                    TMCardDataManager.Instance.AddCard(card);
                }
                
                addButton.SetChangedColorButtonEvent();
            };
            
            addButton.SetChangedColorButtonEvent();
            removeButton.SetChangedColorButtonEvent();
            objectButton.Add(addButton);
            objectButton.Add(removeButton);
            return objectButton;
        }

        internal override void OnDisable()
        {
            _cardNameObservers.ForEach(EditorCoroutineUtility.StopCoroutine);
        }
    }
}
#endif