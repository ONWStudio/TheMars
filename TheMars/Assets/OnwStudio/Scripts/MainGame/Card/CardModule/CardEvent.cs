using System;
namespace TMCard
{
    public enum CardEventState
    {
        DRAW,
        NORMAL,
        TURN_END
    }
    
    public sealed class CardEvent
    {
        private Action<CardEventState> _cardAction;

        public void Invoke(CardEventState eventState)
        {
            _cardAction?.Invoke(eventState);
        }

        public void AddListener(Action<CardEventState> action)
        {
            if (_cardAction is null)
            {
                _cardAction = action;
                return;
            }

            _cardAction += action;
        }

        public void RemoveAllToAddListener(Action<CardEventState> action)
        {
            _cardAction = action;
        }

        public void RemoveListener(Action<CardEventState> action)
        {
            if (_cardAction is null) return;

            _cardAction -= action;
        }

        public void RemoveAllListener()
        {
            _cardAction = null;
        }
    }
}