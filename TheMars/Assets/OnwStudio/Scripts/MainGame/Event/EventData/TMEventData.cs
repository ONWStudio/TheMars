using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Usage.Creator;
using TM.Usage;
using System.Linq;
using TM.Event.Effect.Creator;
using TM.Event.Effect;

namespace TM.Event
{
    public interface ITMEventData
    {
        Sprite EventImage { get; }

        LocalizedString DescriptionTextEvent { get; }
        LocalizedString TopButtonTextEvent { get; }
        LocalizedString BottomButtonTextEvent { get; }
        LocalizedString TitleTextEvent { get; }

        ITMEventData TopReadData { get; }
        ITMEventData BottomReadData { get; }
    }

    public enum TMEventChoice : byte
    {
        TOP = 0,
        BOTTOM = 1
    }

    public class TMEventData : ScriptableObject, ITMEventData
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

        [field: SerializeField, LocalizedString(true, entryKey: "Description"), DisplayAs("[로컬라이징] 이벤트 설명 텍스트")] public LocalizedString DescriptionTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "TopButton"), DisplayAs("[로컬라이징] 위쪽 버튼 텍스트")] public LocalizedString TopButtonTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "BottomButton"), DisplayAs("[로컬라이징] 아래쪽 버튼 텍스트")] public LocalizedString BottomButtonTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "Title"), DisplayAs("[로컬라이징] 타이틀 텍스트")] public LocalizedString TitleTextEvent { get; private set; } = new();

        [field: SerializeField] public bool HasTopEvent { get; private set; } = true;
        [field: SerializeField] public bool HasBottomEvent { get; private set; } = true;

        [SerializeReference, SerializeReferenceDropdown, DisplayAs("위쪽 선택지 효과")] private List<ITMEventEffectCreator> _topEffectCreator = new();
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("아래쪽 선택지 효과")] private List<ITMEventEffectCreator> _bottomEffectCreator = new();

        [SerializeReference, SerializeReferenceDropdown, DisplayAs("위쪽 선택지 소모 자원")] private List<ITMUsageCreator> _topUsageCreators = new();
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("아래쪽 선택지 소모 자원")] private List<ITMUsageCreator> _bottomUsageCreators = new();


        public List<ITMEventEffect> CreateTopEffects()
        {
            return _topEffectCreator
                .Select(creator => creator.CreateEffect())
                .ToList();
        }

        public List<ITMEventEffect> CreateBottomEffects()
        {
            return _bottomEffectCreator
                .Select(creator => creator.CreateEffect())
                .ToList();
        }

        public List<ITMUsage> CreateTopUsages()
        {
            return _topUsageCreators
                .Select(creator => creator.CreateUsage())
                .ToList(); 
        }

        public List<ITMUsage> CreateBottomUsage()
        {
            return _bottomUsageCreators
                .Select(creator => creator.CreateUsage())
                .ToList();
        }
    }
}