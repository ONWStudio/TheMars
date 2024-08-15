using System;
using System.Collections.Generic;
using TMCard.Runtime;
namespace TMCard.Effect
{
    /// <summary>
    /// .. 설치
    /// </summary>
    public sealed class InstallationEffect : TMCardSpecialEffect
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

            public Action<int> OnUpdateStack { get; set; }
            private int _stack;
        }

        // .. UI에 알려주기
        private static readonly Dictionary<string, EventValuePair> _cardStack = new();

        public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
        {
            controller.OnClickEvent.RemoveAllToAddListener(() => onEffect(controller));
            controller.OnDrawEndedEvent.RemoveAllToAddListener(() => onDraw(controller));

            static void onEffect(TMCardController controller)
            {
                if (!_cardStack.TryGetValue(controller.CardData.GetInstanceID().ToString(), out var eventValuePair))
                {
                    eventValuePair = new();
                    _cardStack.Add(controller.CardData.GetInstanceID().ToString(), eventValuePair);
                }

                eventValuePair.Stack++;
                TMCardHelper.Instance.MoveToTomb(controller);
            }

            static void onDraw(TMCardController controller)
            {
                if (!_cardStack.TryGetValue(controller.CardData.GetInstanceID().ToString(), out var eventValuePair) || eventValuePair.Stack <= 0) return;

                eventValuePair.Stack--;
                controller.OnEffectEvent.Invoke();
                TMCardHelper.Instance.DrawUse(controller);
            }
        }

        public static void AddListenerOnUpdateStack(string cardName, Action<int> onUpdateStack)
        {
            if (!_cardStack.TryGetValue(cardName, out var eventValuePair)) return;

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
            if (!_cardStack.TryGetValue(cardName, out var eventValuePair) || eventValuePair.OnUpdateStack is null) return;

            eventValuePair.OnUpdateStack -= onUpdateStack;
        }

        public static void RemoveAllListenerOnUpdataStack(string cardName)
        {
            if (!_cardStack.TryGetValue(cardName, out var eventValuePair) || eventValuePair.OnUpdateStack is null) return;

            eventValuePair.OnUpdateStack = null;
        }

        public InstallationEffect() : base("Installation") {}
    }
}