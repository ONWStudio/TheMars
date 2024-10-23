using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;

namespace TM.Event
{
    public interface ITMEventData
    {
        Sprite EventImage { get; }

        LocalizedString DescriptionTextEvent { get; }
        LocalizedString TopButtonTextEvent { get; }
        LocalizedString BottomButtonTextEvent { get; }
        LocalizedString TitleTextEvent { get; }
        LocalizedString TopEffectTextEvent { get; }
        LocalizedString BottomEffectTextEvent { get; }

        ITMEventData TopReadData { get; }
        ITMEventData BottomReadData { get; }
        
        bool CanFireTop { get; }
        bool CanFireBottom { get; }
    }

    public enum TMEventChoice : byte
    {
        TOP = 0,
        BOTTOM = 1
    }

    public abstract class TMEventData : ScriptableObject, ITMEventData
    {
        public ITMEventData TopReadData => TopEventData;
        public ITMEventData BottomReadData => BottomEventData;

        // .. 추후 이벤트 매니저에서 다음 이벤트를 정할 수 있게끔..
        [field: SerializeField] public TMEventData TopEventData { get; private set; }
        [field: SerializeField] public TMEventData BottomEventData { get; private set; }

        [field: SerializeField, SpritePreview] public Sprite EventImage { get; private set; }

        [field: SerializeField] public LocalizedString DescriptionTextEvent { get; private set; }
        [field: SerializeField] public LocalizedString TopButtonTextEvent { get; private set; }
        [field: SerializeField] public LocalizedString BottomButtonTextEvent { get; private set; }
        [field: SerializeField] public LocalizedString TitleTextEvent { get; private set; }
        [field: SerializeField] public LocalizedString TopEffectTextEvent { get; private set; }
        [field: SerializeField] public LocalizedString BottomEffectTextEvent { get; private set; }
        
        public abstract bool CanFireTop { get; }
        public abstract bool CanFireBottom { get; }
        
        public abstract List<object> TopEffectLocalizedArguments { get; }
        public abstract List<object> BottomEffectLocalizedArguments { get; }
        
        public void InvokeEvent(TMEventChoice eventChoice)
        {
            if (eventChoice == TMEventChoice.TOP)
            {
                TriggerLeftEvent();
            }
            else
            {
                TriggerRightEvent();
            }
        }

        protected abstract void TriggerLeftEvent();
        protected abstract void TriggerRightEvent();
    }
}