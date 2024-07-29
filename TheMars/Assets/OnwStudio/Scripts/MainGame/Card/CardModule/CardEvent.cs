using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    public sealed class CardEvent
    {
        private Action _cardAction = null;

        public void Invoke()
        {
            _cardAction?.Invoke();
        }

        public void AddListener(Action action)
        {
            if (_cardAction is null)
            {
                _cardAction = action;
                return;
            }

            _cardAction += action;
        }

        public void RemoveAllToAddListener(Action action)
        {
            _cardAction = action;
        }

        public void RemoveListener(Action action)
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