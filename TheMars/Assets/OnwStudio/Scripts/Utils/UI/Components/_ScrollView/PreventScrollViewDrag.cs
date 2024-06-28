using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Onw.UI.Components
{
    // .. 스크롤 뷰 이벤트를 일어나지 않게 합니다.
    public class PreventScrollViewDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<PointerEventData> DragEvent { get; set; } = eventData => { };
        public Action<PointerEventData> BeginDragEvent { get; set; } = eventData => { };
        public Action<PointerEventData> EndDragEvent { get; set; } = eventData => { };
        public ScrollRect ScrollView { get; private set; }

        private void Awake() => ScrollView = GetComponent<ScrollRect>();
        public void OnDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
            DragEvent.Invoke(eventData);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
            BeginDragEvent.Invoke(eventData);
        }
        public void OnEndDrag(PointerEventData eventData) => EndDragEvent.Invoke(eventData);
    }
}
