using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace TM.Event
{
    public interface ITMEvent
    {
        event UnityAction<TMEventChoice> OnFireEvent;
        
        ITMEvent Left { get; }
        ITMEvent Right { get; }
    }

    public enum TMEventChoice : byte
    {
        LEFT = 0,
        RIGHT = 1
    }
    

    public abstract class TMEventData : ScriptableObject, ITMEvent
    {
        public ITMEvent Left => LeftEvent;
        public ITMEvent Right => RightEvent;

        // .. 추후 이벤트 매니저에서 다음 이벤트를 정할 수 있게끔..
        [field: SerializeField] public TMEventData LeftEvent  { get; private set; }
        [field: SerializeField] public TMEventData RightEvent { get; private set; }
        
        [field: SerializeField, SpritePreview] public Sprite EventImage { get; private set; }

        public abstract string Description { get; }
        public abstract string LeftDescription { get; }
        public abstract string RightDescription { get; }
        public abstract string HeaderDescription { get; }

        public event UnityAction<TMEventChoice> OnFireEvent;
        
        public void InvokeEvent(TMEventChoice eventChoice)
        {
            if (eventChoice == TMEventChoice.LEFT)
            {
                TriggerLeftEvent();
            }
            else
            {
                TriggerRightEvent();                
            }
            
            OnFireEvent?.Invoke(eventChoice);
        }
        
        protected abstract void TriggerLeftEvent();
        protected abstract void TriggerRightEvent();
    }
}
