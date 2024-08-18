using Onw.Feedback;
using Onw.ServiceLocator;
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

        public override void ApplyEffect(TMCardController card, ITMEffectTrigger trigger)
        {
            if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service)) return;

            card.OnClickEvent.RemoveAllToAddListener(() => onEffect(card));
            card.OnDrawEndedEvent.RemoveAllToAddListener(() => onDraw(service, card));

            static void onEffect(TMCardController card)
            {
                if (!_cardStack.TryGetValue(card.CardData.GetInstanceID().ToString(), out var eventValuePair))
                {
                    eventValuePair = new();
                    _cardStack.Add(card.CardData.GetInstanceID().ToString(), eventValuePair);
                }

                eventValuePair.Stack++;
                card.MoveToTomb();
            }

            static void onDraw(ITMCardService service, TMCardController card)
            {
                if (!_cardStack.TryGetValue(card.CardData.GetInstanceID().ToString(), out var eventValuePair) || eventValuePair.Stack <= 0) return;

                eventValuePair.Stack--;

                service.FeedbackPlayer.EnqueueEvent(
                    card.GetMoveToScreenCenterEvent(),
                    FeedbackCreator.CreateUnityEvent(() =>
                    {
                        card.OnEffectEvent.Invoke();
                        service.FeedbackPlayer.EnqueueEventToHead(service.CardHandController.GetSortCardsFeedbacks());
                    }));
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