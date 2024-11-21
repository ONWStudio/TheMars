using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using TM.Cost.Creator;
using TM.Cost;
using System.Linq;
using TM.Event.Effect.Creator;
using TM.Event.Effect;
using UnityEngine.Serialization;

namespace TM.Event
{
    public interface ITMEventData
    {
        string ID { get; }
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

        [field: SerializeField, ReadOnly] public string ID { get; private set; } = Guid.NewGuid().ToString();
        [field: SerializeField, DisplayAs("위쪽 선택지")] public TMEventData TopEventData { get; private set; }
        [field: SerializeField, DisplayAs("아래쪽 선택지")] public TMEventData BottomEventData { get; private set; }

        [field: SerializeField, DisplayAs("이벤트 대표 이미지"), SpritePreview] public Sprite EventImage { get; private set; }

        [field: SerializeField, LocalizedString(true, entryKey: "Description"), DisplayAs("[로컬라이징] 이벤트 설명 텍스트")] public LocalizedString DescriptionTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "TopButton"), DisplayAs("[로컬라이징] 위쪽 버튼 텍스트")] public LocalizedString TopButtonTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "BottomButton"), DisplayAs("[로컬라이징] 아래쪽 버튼 텍스트")] public LocalizedString BottomButtonTextEvent { get; private set; } = new();
        [field: SerializeField, LocalizedString(true, entryKey: "Title"), DisplayAs("[로컬라이징] 타이틀 텍스트")] public LocalizedString TitleTextEvent { get; private set; } = new();

        [field: SerializeField] public bool HasTopEvent { get; private set; } = true;
        [field: SerializeField] public bool HasBottomEvent { get; private set; } = true;

        [FormerlySerializedAs("_topEffectCreator")]
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("위쪽 선택지 효과")] private List<ITMEventEffectCreator> _topEffectCreators = new();
        [FormerlySerializedAs("_bottomEffectCreator")]
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("아래쪽 선택지 효과")] private List<ITMEventEffectCreator> _bottomEffectCreators = new();

        [FormerlySerializedAs("_topUsageCreators")]
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("위쪽 선택지 소모 자원")] private List<ITMCostCreator> _topCostCreators = new();
        [FormerlySerializedAs("_bottomUsageCreators")]
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("아래쪽 선택지 소모 자원")] private List<ITMCostCreator> _bottomCostCreators = new();

        public List<ITMEventEffect> CreateTopEffects()
        {
            return _topEffectCreators
                .Select(creator => creator.CreateEffect())
                .ToList();
        }

        public List<ITMEventEffect> CreateBottomEffects()
        {
            return _bottomEffectCreators
                .Select(creator => creator.CreateEffect())
                .ToList();
        }

        public List<ITMCost> CreateTopCosts()
        {
            return _topCostCreators
                .Select(creator => creator.CreateCost())
                .ToList(); 
        }

        public List<ITMCost> CreateBottomCosts()
        {
            return _bottomCostCreators
                .Select(creator => creator.CreateCost())
                .ToList();
        }
    }
}