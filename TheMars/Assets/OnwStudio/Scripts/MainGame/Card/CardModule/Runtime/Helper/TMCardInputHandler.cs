using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TMCard.Runtime
{
    /// <summary>
    /// .. Card UI의 입력 이벤트를 받아옵니다
    /// </summary>
    public sealed class TMCardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Action<PointerEventData> _enterAction;
        private Action<PointerEventData> _exitAction;
        private Action<PointerEventData> _downAction;
        private Action<PointerEventData> _upAction;
        private Action<PointerEventData> _dragAction;

        public void AddListenerPointerEnterAction(Action<PointerEventData> action)
            => _enterAction += action;

        public void AddListenerPointerExitAction(Action<PointerEventData> action)
            => _exitAction += action;

        public void AddListenerPointerDownAction(Action<PointerEventData> action)
            => _downAction += action;

        public void AddListenerPointerUpAction(Action<PointerEventData> action)
            => _upAction += action;

        public void AddListenerPointerDragAction(Action<PointerEventData> action)
            => _dragAction += action;

        public void RemoveListenerPointerEnterAction(Action<PointerEventData> action)
            => _enterAction -= action;

        public void RemoveListenerPointerExitAction(Action<PointerEventData> action)
            => _exitAction -= action;

        public void RemoveListenerPointerDownAction(Action<PointerEventData> action)
            => _downAction -= action;

        public void RemoveListenerPointerDragAction(Action<PointerEventData> action)
            => _dragAction -= action;

        public void RemoveListenerPointerUpAction(Action<PointerEventData> action)
            => _upAction -= action;

        public void RemoveAllListenerPointerEnterAction()
            => _enterAction = null;

        public void RemoveAllListenerPointerExitAction()
            => _exitAction = null;
        
        public void RemoveAllListenerPointerDownAction()
            => _downAction = null;

        public void RemoveAllListenerPointerUpAction()
            => _upAction = null;

        public void RemoveAllListenerPointerDragAction()
            => _dragAction = null;

        public void OnPointerEnter(PointerEventData eventData)
            => _enterAction?.Invoke(eventData);

        public void OnPointerExit(PointerEventData eventData)
            => _exitAction?.Invoke(eventData);
        
        public void OnPointerUp(PointerEventData eventData)
            => _upAction?.Invoke(eventData);
        
        public void OnDrag(PointerEventData eventData)
            => _dragAction?.Invoke(eventData);
        
        public void OnPointerDown(PointerEventData eventData)
            => _downAction?.Invoke(eventData);
    }
}