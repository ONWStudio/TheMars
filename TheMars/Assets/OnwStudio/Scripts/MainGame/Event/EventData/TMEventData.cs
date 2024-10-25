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
        [field: InfoBox("효과 텍스트 작성 팁 : 효과 텍스트 작성시 어떤 값을 불러와야 할때는 \"{Steel} 강철 소모\"와 같은 형태로 작성되어야 합니다 아래는 불러와야 할 값 별 Key입니다 \n \n" + 
                        "\t 마르스 리튬 : MarsLithium \n" +
                        "\t 인구 : Population \n" +
                        "\t 크레딧 : Credit \n" +
                        "\t 강철 : Steel \n" +
                        "\t 식물 : Plants \n" +
                        "\t 점토 : Clay \n" +
                        "\t 전기 : Electricity \n \n" +
                        "여기에 해당 값이 더할거냐 뺄거냐에 따라 접미사를 부여합니다 \n \n" +
                        "\t 더하기 : Add \n" +
                        "\t 빼기 : Subtract \n \n" +
                        "최종적으로 마르스 리튬 소모량일 경우는 : {MarsLithiumSubtract} 가 됩니다 \n" +
                        "그 외 효과별 고유한 값이 있는경우 해당 효과에 부연 설명으로 작성해둡니다")]
        [field: SerializeField, DisplayAs("위쪽 선택지")] public TMEventData TopEventData { get; private set; }
        [field: SerializeField, DisplayAs("아래쪽 선택지")] public TMEventData BottomEventData { get; private set; }

        [field: SerializeField, DisplayAs("이벤트 대표 이미지"), SpritePreview] public Sprite EventImage { get; private set; }

        [field: SerializeField, DisplayAs("[로컬라이징] 이벤트 설명 텍스트")] public LocalizedString DescriptionTextEvent { get; private set; }
        [field: SerializeField, DisplayAs("[로컬라이징] 위쪽 버튼 텍스트")] public LocalizedString TopButtonTextEvent { get; private set; }
        [field: SerializeField, DisplayAs("[로컬라이징] 아래쪽 버튼 텍스트")] public LocalizedString BottomButtonTextEvent { get; private set; }
        [field: SerializeField, DisplayAs("[로컬라이징] 타이틀 텍스트")] public LocalizedString TitleTextEvent { get; private set; }
        [field: SerializeField, DisplayAs("[로컬라이징] 위쪽 선택지 효과 텍스트")] public LocalizedString TopEffectTextEvent { get; private set; }
        [field: SerializeField, DisplayAs("[로컬라이징] 아래쪽 선택지 효과 텍스트")] public LocalizedString BottomEffectTextEvent { get; private set; }
        
        public abstract bool CanFireTop { get; }
        public abstract bool CanFireBottom { get; }
        
        public abstract Dictionary<string, object> TopEffectLocalizedArguments { get; }
        public abstract Dictionary<string, object> BottomEffectLocalizedArguments { get; }
        
        public void InvokeEvent(TMEventChoice eventChoice)
        {
            if (eventChoice == TMEventChoice.TOP)
            {
                TriggerTopEvent();
            }
            else
            {
                TriggerBottomEvent();
            }
        }

        protected abstract void TriggerTopEvent();
        protected abstract void TriggerBottomEvent();
    }
}