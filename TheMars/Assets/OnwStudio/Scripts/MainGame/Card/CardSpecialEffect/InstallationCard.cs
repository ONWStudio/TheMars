using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// .. 설치
/// </summary>
[SerializeReferenceDropdownName("설치")]
public sealed class InstallationCard : ICardSpecialEffect
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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void initialize()
    {
        _cardStack.Clear();
    }

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        // .. 효과 발동없이 화면 중앙 이동 후 무덤 이동 (무덤 이동은 카드 기본 효과이므로 생략)
        cardController.UseStartedState = () => onEffect(cardController);
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
        if (!_cardStack.TryGetValue(cardName, out EventValuePair eventValuePair) ||
            eventValuePair.OnUpdateStack is null ||
            !eventValuePair.OnUpdateStack.GetInvocationList().Any(callback => (Action<int>)callback == onUpdateStack)) return;

        eventValuePair.OnUpdateStack -= onUpdateStack;
    }

    private void onEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.OnMoveToScreenCenter.Invoke(cardController, false);

        if (!_cardStack.ContainsKey(cardController.CardData.CardName))
        {
            _cardStack.Add(cardController.CardData.CardName, new());
        }

        _cardStack[cardController.CardData.CardName].Stack++;
    }

    private void onDraw<T>(T cardController) where T : TMCardController<T>
    {
        if (!_cardStack.ContainsKey(cardController.CardData.CardName) || _cardStack[cardController.CardData.CardName].Stack <= 0) return;

        _cardStack[cardController.CardData.CardName].Stack--;
        cardController.CardData.UseCard(cardController.gameObject);
        cardController.OnDrawUse.Invoke(cardController);
    }
}
