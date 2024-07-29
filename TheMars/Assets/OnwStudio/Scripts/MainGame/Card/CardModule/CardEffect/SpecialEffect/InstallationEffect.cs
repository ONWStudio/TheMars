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
    public sealed class InstallationEffect : ITMCardSpecialEffect
    {
        private sealed class EventValuePair
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

        public string Label => TMLocalizationManager.Instance.GetSpecialEffectLabel("설치");

        // .. UI에 알려주기
        private static readonly Dictionary<string, EventValuePair> _cardStack = new();

        public void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => onEffect(controller));
            controller.OnDrawEndedEvent.RemoveAllToAddListener(() => onDraw(controller));
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

        private void onDraw(TMCardController controller)
        {
            if (!_cardStack.ContainsKey(controller.CardData.CardName) || _cardStack[controller.CardData.CardName].Stack <= 0) return;

            _cardStack[controller.CardData.CardName].Stack--;
            controller.OnEffectEvent.Invoke();
            TMCardGameManager.Instance.DrawUse(controller);
        }
    }
}