using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;
using TMCard;

namespace TMCard.Effect
{
    /// <summary>
    /// .. 설치
    /// </summary>
    [SerializeReferenceDropdownName("설치"), Substitution("설치")]
    public sealed class InstallationCard : ITMCardSpecialEffect
    {
        private class EventValuePair
        {
            public int Stack
            {
                get => _stack;
                set
                {
                    _stack = value;
                    OnUpdateStack?.Invoke(_stack);
                }
            }

            public Action<int> OnUpdateStack { get; set; } = null;

            private int _stack = 0;
        }

        // .. UI에 알려주기
        private static readonly Dictionary<string, EventValuePair> _cardStack = new();
        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("설치");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void initialize()
        {
            _cardStack.Clear();
        }

        public void ApplyEffect(TMCardController cardController)
        {
            // .. 효과 발동없이 화면 중앙 이동 후 무덤 이동 (무덤 이동은 카드 기본 효과이므로 생략)
            cardController.UseState = () => onEffect(cardController);
            cardController.DrawEndedState = () => onDraw(cardController);
        }

        public static void AddListenerOnUpdateStack(string cardName, Action<int> onUpdateStack)
        {
            if (!_cardStack.TryGetValue(cardName, out EventValuePair eventValuePair)) return;

            if (eventValuePair.OnUpdateStack is null)
            {
                eventValuePair.OnUpdateStack = onUpdateStack;
            }
            else
            {
                eventValuePair.OnUpdateStack += onUpdateStack;
            }
        }

        public static void RemoveListenerOnUpdateStack(string cardName, Action<int> onUpdateStack)
        {
            if (!_cardStack.TryGetValue(cardName, out EventValuePair eventValuePair) || eventValuePair.OnUpdateStack is null) return;

            eventValuePair.OnUpdateStack -= onUpdateStack;
        }

        public static void RemoveAllListenerOnUpdataStack(string cardName)
        {
            if (!_cardStack.TryGetValue(cardName, out EventValuePair eventValuePair) || eventValuePair.OnUpdateStack is null) return;

            eventValuePair.OnUpdateStack = null;
        }

        private void onEffect(TMCardController cardController)
        {
            if (!_cardStack.TryGetValue(cardController.CardData.CardName, out EventValuePair eventValuePair))
            {
                eventValuePair = new();
                _cardStack.Add(cardController.CardData.CardName, eventValuePair);
            }

            eventValuePair.Stack++;

            TMCardGameManager.Instance.MoveToTomb(cardController);
        }

        private void onDraw(TMCardController cardController)
        {
            if (!_cardStack.ContainsKey(cardController.CardData.CardName) || _cardStack[cardController.CardData.CardName].Stack <= 0) return;

            _cardStack[cardController.CardData.CardName].Stack--;
            cardController.CardData.ApplyEffect(cardController);
            TMCardGameManager.Instance.DrawUse(cardController);
        }
    }
}