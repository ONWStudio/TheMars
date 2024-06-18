//#if UNITY_EDITOR
//using System;
//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using EditorTool;
//using static EditorTool.EditorTool;

//namespace TheMarsGUITool
//{
//    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
//    {
//        private sealed class CardGUIDrawer : IGUIDrawer
//        {
//            private const string DATA_PATH = "Assets/OnwStudio/ScriptableObject/Cards";

//            public int Page { get; set; }
//            public int MaxPage => _cards.Count;

//            public bool HasErrors { get; set; } = false;
//            public bool IsSuccess { get; set; } = false;
//            public string Message { get; set; } = string.Empty;

//            private GUIStyle _cardBoxStyle = null;

//            private readonly EditorDropdownController<ICardCondition> _conditionSelector = new(
//                "추가 조건",
//                ReflectionHelper
//                    .GetChildClassesFromType<ICardCondition>()
//                    .ToDictionary(condition => condition.GetType().Name, condition => condition));

//            private readonly EditorDropdownController<string> _localizingDropdown = new("지역", new() { { "한글", "ko-KR" }, { "공통 (영어)", "en-US" } });
//            private readonly ToggleEnumerator<ICardCondition> _conditionEnumerator = new();

//            private readonly List<TMCardData> _cards = new();
//            private string _newCardName = string.Empty;

//            public void Awake()
//            {
//                _cards.AddRange(DataHandler<TMCardData>
//                    .LoadAllScriptableObjects());

//                _cards.ForEach(insertLocalizingOptions);
//                Page = 1;
//            }

//            public void OnEnable()
//            {
                
//            }

//            public void OnDraw()
//            {
//                TMCardData cardData = null;

//                if (!_conditionEnumerator.IsInitializeOnGetDataList)
//                {
//                    _conditionEnumerator.SetOnGetDataList(() => cardData ? cardData.AdditionalCondition : null);
//                }

//                if (!_conditionEnumerator.IsInitializeGUIStyle)
//                {
//                    _conditionEnumerator.InitializeGUIStyle(Color.white, Color.gray);
//                }

//                if (_cardBoxStyle is null)
//                {
//                    _cardBoxStyle = new(GUI.skin.box);
//                    _cardBoxStyle.normal.background = GetTexture2D(Color.black, _cardBoxStyle);
//                }

//                _localizingDropdown.Dropdown(name => { });

//                ActionEditorHorizontal(() =>
//                {
//                    bool isCreateNewCard = GUILayout.Button("새 카드 추가");

//                    _newCardName = EditorGUILayout.TextField("Card Name : ", _newCardName);

//                    if (isCreateNewCard)
//                    {
//                        if (string.IsNullOrEmpty(_newCardName))
//                        {
//                            Message = "카드의 이름이 정해지지 않았습니다";
//                        }
//                        else
//                        {
//                            _cards.Add(DataHandler<TMCardData>.CreateScriptableObject(DATA_PATH, $"Card_No.{_cards.Count + 1}"));
//                            insertLocalizingOptions(_cards[^1]);
//                            _cards[^1].CardNames[_localizingDropdown.SelectedItem.Value] = _newCardName;
//                            _cards[^1].Guid = Guid.NewGuid().ToString();
//                            DataHandler<TMCardData>.SaveData(_cards[^1]);
//                        }
//                    }
//                });

//                int page = Page - 1;

//                if (page >= 0 && _cards.Count > page)
//                {
//                    cardData = _cards[page];

//                    ActionEditorVertical(() =>
//                    {
//                        ActionEditorHorizontal(() =>
//                        {
//                            EditorGUILayout.LabelField($"Guid : {cardData.Guid}");

//                            if (GUILayout.Button("new GUID"))
//                            {
//                                cardData.Guid = Guid.NewGuid().ToString();
//                            }
//                        });

//                        cardData.CardNames[_localizingDropdown.SelectedItem.Value] = EditorGUILayout.TextField(
//                            "Card Name",
//                            cardData.CardNames[_localizingDropdown.SelectedItem.Value]);

//                        IsVaildName(cardData);

//                        EditorGUILayout.LabelField("소모 자원");
//                        ActionEditorHorizontal(() =>
//                        {
//                            cardData.MarsLithium = Mathf.Clamp(
//                                EditorGUILayout.IntField("마르스 리튬 : ", cardData.MarsLithium),
//                                0,
//                                int.MaxValue);
//                        });
//                    }, _cardBoxStyle);

//                    ActionEditorVertical(() =>
//                    {
//                        ActionEditorHorizontal(() =>
//                        {
//                            ActionEditorVertical(() =>
//                            {
//                                _conditionSelector.Dropdown(additionalCondition =>
//                                {
//                                    if (cardData.AdditionalCondition.Any(condition => condition.GetType().Name == additionalCondition.GetType().Name)) return;

//                                    cardData.AdditionalCondition.Add(Activator.CreateInstance(additionalCondition.GetType()) as ICardCondition);
//                                });

//                                _conditionEnumerator.SelectEnumeratedToggles(additionalCondition => additionalCondition.GetType().Name);
//                            });

//                            ActionEditorVertical(() =>
//                            {

//                            });
//                        });
//                    }, _cardBoxStyle);
//                }
//            }

//            public void LoadDataFromLocal()
//            {
//                _cards.Clear();
//                _cards.AddRange(DataHandler<TMCardData>
//                    .LoadAllScriptableObjects()
//                    .OrderBy(card => card.Guid));
//            }

//            public void SaveDataToLocal()
//            {

//            }

//            private void IsVaildName(TMCardData cardData)
//            {
//                if (cardData.CardNames.Values.All(name => !string.IsNullOrEmpty(name))) return;

//                Message = "카드의 이름을 입력해주세요";
//            }

//            private void insertLocalizingOptions(TMCardData cardData)
//            {
//                foreach (string option in _localizingDropdown.Options.Values)
//                {
//                    if (!cardData.Descriptions.ContainsKey(option))
//                    {
//                        cardData.Descriptions.Add(option, "");
//                    }

//                    if (!cardData.CardNames.ContainsKey(option))
//                    {
//                        cardData.CardNames.Add(option, "");
//                    }
//                }
//            }
//        }
//    }
//}
//#endif