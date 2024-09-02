using System;
using Onw.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace TMCard.Runtime
{
    /// <summary>
    /// .. Card UI의 입력 이벤트를 받아옵니다
    /// </summary>
    public sealed class TMCardInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public IUnityEventListenerModifier<PointerEventData> EnterAction => _enterAction;
        public IUnityEventListenerModifier<PointerEventData> ExitAction => _exitAction;
        public IUnityEventListenerModifier<PointerEventData> DownAction => _downAction;
        public IUnityEventListenerModifier<PointerEventData> UpAction => _upAction;
        public IUnityEventListenerModifier<PointerEventData> DragAction => _dragAction;
        
        [SerializeField] private SafeUnityEvent<PointerEventData> _enterAction = new();
        [SerializeField] private SafeUnityEvent<PointerEventData> _exitAction = new();
        [SerializeField] private SafeUnityEvent<PointerEventData> _downAction = new();
        [SerializeField] private SafeUnityEvent<PointerEventData> _upAction = new();
        [SerializeField] private SafeUnityEvent<PointerEventData> _dragAction = new();

        public void OnPointerEnter(PointerEventData eventData) => _enterAction.Invoke(eventData);
        public void OnPointerExit(PointerEventData eventData) => _exitAction.Invoke(eventData);
        public void OnPointerUp(PointerEventData eventData) => _upAction.Invoke(eventData);
        public void OnDrag(PointerEventData eventData) => _dragAction.Invoke(eventData);
        public void OnPointerDown(PointerEventData eventData) => _downAction.Invoke(eventData);
    }
}