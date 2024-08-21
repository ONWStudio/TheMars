using System.Linq;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Localization;
using TMCard.AdditionalCondition;
using TMCard.Effect;
using TMCard.Runtime;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        /// <summary>
        /// .. 카드 전체와 공유하는 스택 ID
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<StackID>k__BackingField"), Tooltip("Stack ID")]
        public int StackID { get; private set; }

        /// <summary>
        /// .. 소모 재화 종류
        /// </summary>
        [field: Space]
        [field: SerializeField, FormerlySerializedAs("<RequiredResource>k__BackingField"), DisplayAs("소모 재화 종류"), Tooltip("소모 재화 종류")]
        public TMRequiredResource RequiredResource { get; private set; } = TMRequiredResource.TERA;

        /// <summary>
        /// .. 사용될 재화
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<Resource>k__BackingField"), DisplayAs("소모 재화량"), Tooltip("소모 재화량")]
        public int Resource { get; private set; }

        /// <summary>
        /// .. 카드의 종류
        /// </summary>
        [field: Space]
        [field: SerializeField, FormerlySerializedAs("<CardKind>k__BackingField"), DisplayAs("카드 종류"), Tooltip("카드의 종류")]
        public TMCardKind CardKind { get; private set; } = TMCardKind.NONE;

        /// <summary>
        /// .. 카드의 등급
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardGrade>k__BackingField"), DisplayAs("등급"), Tooltip("카드의 등급")]
        public TMCardGrade CardGrade { get; private set; } = TMCardGrade.NORMAL;

        /// <summary>
        /// .. 카드의 그룹
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardGroup>k__BackingField"), DisplayAs("그룹"), Tooltip("카드의 그룹")]
        public TMCardGroup CardGroup { get; private set; } = TMCardGroup.COMMON;

        [field: SerializeField, SpritePreview(128f)] public Sprite CardImage { get; private set; }
        [field: SerializeField, FormerlySerializedAs("<IsCustomDescription>k__BackingField"), DisplayAs("커스텀 설명"), Tooltip("체크 시 기본 설명이 나오지 않습니다")]
        public string CustomDescription { get; private set; }
        
        public string CardName => _localizedCardName.TryGetLocalizedString(out string cardName) ? cardName : "";

        [FormerlySerializedAs("effectCreators")]
        [SerializeReference, DisplayAs("카드 효과"), Tooltip("카드 효과 리스트"), SerializeReferenceDropdown]
        private List<ITMEffectCreator> _effectCreators = new();

        [FormerlySerializedAs("additionalConditions")]
        [SerializeReference, DisplayAs("추가 조건"), Tooltip("카드 추가 조건 리스트"), SerializeReferenceDropdown]
        private List<ITMCardAdditionalCondition> _additionalConditions = new();

        [SerializeField] private LocalizedString _localizedCardName;
        
        public void ApplyEffect(TMCardController controller)
        {
            controller.SetEffect(_effectCreators
                .Select(effectCreator => effectCreator?.CreateEffect()));
        }

        /// <summary>
        /// .. 카드의 사용 전 카드가 사용가능한 상태인지 확인합니다
        /// </summary>
        /// <param name="resource"> .. 현재 재화량 </param>
        /// <returns></returns>
        public bool IsAvailable(int resource)
        {
            return Resource <= resource &&
                _additionalConditions.All(additionalCondition => additionalCondition?.AdditionalCondition ?? false);
        }
    }
}