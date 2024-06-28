using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMCard.UI
{
    /// <summary>
    /// .. Card UI의 입력 이벤트를 받아옵니다
    /// </summary>
    public sealed class TMCardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Action<PointerEventData> _enterAction = null;
        private Action<PointerEventData> _exitAction = null;
        private Action<PointerEventData> _clickAction = null;

        public void AddListenerPointerEnterAction(Action<PointerEventData> action)
            => _enterAction += action;

        public void AddListenerPointerExitAction(Action<PointerEventData> action)
            => _exitAction += action;

        public void AddListenerPointerClickAction(Action<PointerEventData> action)
            => _clickAction += action;

        public void RemoveListenerPointerEnterAction(Action<PointerEventData> action)
            => _enterAction -= action;

        public void RemoveListenerPointerExitAction(Action<PointerEventData> action)
            => _exitAction -= action;

        public void RemoveListenerPointerClickAction(Action<PointerEventData> action)
            => _clickAction -= action;

        public void RemoveAllListenerPointerEnterAction()
            => _enterAction = null;

        public void RemoveAllListenerPointerExitAction()
            => _exitAction = null;

        public void RemoveAllListenerPointerClickAction()
            => _clickAction = null;

        public void OnPointerClick(PointerEventData eventData)
            => _clickAction?.Invoke(eventData);

        public void OnPointerEnter(PointerEventData eventData)
            => _enterAction?.Invoke(eventData);

        public void OnPointerExit(PointerEventData eventData)
            => _exitAction?.Invoke(eventData);
    }
}