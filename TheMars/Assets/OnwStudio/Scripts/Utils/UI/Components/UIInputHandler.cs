using System;
using Onw.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Onw.UI.Components
{
    /// <summary>
    /// .. UI의 입력 이벤트를 받아옵니다
    /// </summary>
    public sealed class UIInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public event UnityAction<PointerEventData> EnterAction
        {
            add => _enterAction.AddListener(value);
            remove => _enterAction.RemoveListener(value);
        }
        
        public event UnityAction<PointerEventData> ExitAction
        {
            add => _exitAction.AddListener(value);
            remove => _exitAction.RemoveListener(value);
        }
        
        public event UnityAction<PointerEventData> DownAction
        {
            add => _downAction.AddListener(value);
            remove => _downAction.RemoveListener(value);
        }
        
        public event UnityAction<PointerEventData> UpAction
        {
            add => _upAction.AddListener(value);
            remove => _upAction.RemoveListener(value);
        }
        
        public event UnityAction<PointerEventData> DragAction
        {
            add => _dragAction.AddListener(value);
            remove => _dragAction.RemoveListener(value);
        }
        
        [SerializeField] private UnityEvent<PointerEventData> _enterAction = new();
        [SerializeField] private UnityEvent<PointerEventData> _exitAction = new();
        [SerializeField] private UnityEvent<PointerEventData> _downAction = new();
        [SerializeField] private UnityEvent<PointerEventData> _upAction = new();
        [SerializeField] private UnityEvent<PointerEventData> _dragAction = new();

        public void OnPointerEnter(PointerEventData eventData) => _enterAction.Invoke(eventData);
        public void OnPointerExit(PointerEventData eventData) => _exitAction.Invoke(eventData);
        public void OnPointerUp(PointerEventData eventData) => _upAction.Invoke(eventData);
        public void OnDrag(PointerEventData eventData) => _dragAction.Invoke(eventData);
        public void OnPointerDown(PointerEventData eventData) => _downAction.Invoke(eventData);
    }
}