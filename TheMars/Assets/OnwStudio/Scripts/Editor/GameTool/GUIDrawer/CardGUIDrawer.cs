#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorTool.EditorTool;
using UnityEditor.VersionControl;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
    {
        internal sealed class CardGUIDrawer : IGUIDrawer
        {
            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Cards";

            public int Page { get; set; }
            public int MaxPage => _cards.Count;

            public bool HasErrors { get; set; } = false;
            public bool IsSuccess { get; set; } = false;
            public string Message { get; set; } = string.Empty;

            private GUIStyle _cardBoxStyle = null;

            private readonly List<Card> _cards = new();
            private string _newCardName = string.Empty;

            public void Awake()
            {
                _cards.AddRange(DataHandler<Card>
                    .LoadAllScriptableObjects()
                    .OrderBy(card => card.No));

                Page = 1;
            }

            public void OnEnable()
            {

            }

            public void OnDraw()
            {
                if (_cardBoxStyle is null)
                {
                    _cardBoxStyle = new(GUI.skin.box);
                    _cardBoxStyle.normal.background = GetTexture2D(Color.black, _cardBoxStyle);
                }

                ActionEditorHorizontal(() =>
                {
                    bool isCreateNewCard = GUILayout.Button("새 카드 추가");

                    _newCardName = EditorGUILayout.TextField("Card Name : ", _newCardName);

                    if (isCreateNewCard)
                    {
                        if (string.IsNullOrEmpty(_newCardName))
                        {
                            Message = "카드의 이름이 정해지지 않았습니다";
                            return;
                        }

                        if (_cards.Any(card => card.CardName == _newCardName))
                        {
                            Message = "이미 해당 카드의 이름이 있습니다";
                            return;
                        }

                        _cards.Add(DataHandler<Card>.CreateScriptableObject(DATA_PATH, $"Card_No.{_cards.Count + 1}"));
                        _cards[^1].CardName = _newCardName;
                        _cards[^1].No = _cards.Count;
                    }
                });

                int page = Page - 1;

                if (page >= 0 && _cards.Count > page)
                {
                    ActionEditorVertical(() =>
                    {
                        EditorGUILayout.LabelField($"No : {_cards[page].No}");

                        string newName = EditorGUILayout.TextField("Card Name", _cards[page].CardName);
                        setNewName(_cards[page], newName);

                        EditorGUILayout.LabelField("소모 자원");
                        ActionEditorHorizontal(() =>
                        {
                            EditorGUILayout.LabelField("마르스 리튬 : ");
                            _cards[page].MarsLithium = Mathf.Clamp(EditorGUILayout.IntField(_cards[page].MarsLithium), 0, int.MaxValue);

                            EditorGUILayout.LabelField("인구 : ");
                            _cards[page].People = Mathf.Clamp(EditorGUILayout.IntField(_cards[page].People), 0, int.MaxValue);
                        });
                    }, _cardBoxStyle);
                }
            }

            public void LoadDataFromLocal()
            {
                _cards.Clear();
                _cards.AddRange(DataHandler<Card>
                    .LoadAllScriptableObjects()
                    .OrderBy(card => card.No));
            }

            public void SaveDataToLocal()
            {

            }

            private void setNewName(Card card, string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Message = "카드의 이름을 입력해주세요";
                    return;
                }

                if (_cards.Any(someCard => someCard != card && someCard.CardName == name))
                {
                    Message = "해당 이름의 카드가 이미 존재합니다";
                    return;
                }

                card.CardName = name;
            }
        }
    }
}
#endif