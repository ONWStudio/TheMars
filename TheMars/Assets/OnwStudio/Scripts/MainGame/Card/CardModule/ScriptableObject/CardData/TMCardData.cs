using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Localization.Tables;
using Onw.Attribute;
using Onw.Interface;
using Onw.Localization;
using TMCard.AddtionalCondition;
using TMCard.Effect;
using TMCard.Runtime;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        /// <summary>
        /// .. 카드의 고유 ID
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<Guid>k__BackingField"), Tooltip("카드의 고유 ID"), ReadOnly]
        public string Guid { get; private set; } = System.Guid.NewGuid().ToString();

        /// <summary>
        /// .. 카드 전체와 공유하는 스택 ID
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<StackID>k__BackingField"), Tooltip("Stack ID")]
        public int StackID { get; private set; } = 0;

        /// <summary>
        /// .. 같은 그룹간에 공유하는 스택 ID
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<GroupStackID>k__BackingField"), Tooltip("그룹 Stack ID")]
        public int GroupStackID { get; private set; } = 0;

        [field: Space]
        /// <summary>
        /// .. 소모 재화 종류
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<RequiredResource>k__BackingField"), DisplayAs("소모 재화 종류"), Tooltip("소모 재화 종류")]
        public TM_REQUIRED_RESOURCE RequiredResource { get; private set; } = TM_REQUIRED_RESOURCE.TERA;

        /// <summary>
        /// .. 사용될 재화
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<Resource>k__BackingField"), DisplayAs("소모 재화량"), Tooltip("소모 재화량")]
        public int Resource { get; private set; } = 0;

        [field: Space]
        /// <summary>
        /// .. 카드의 종류
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardKind>k__BackingField"), DisplayAs("카드 종류"), Tooltip("카드의 종류")]
        public TM_CARD_KIND CardKind { get; private set; } = TM_CARD_KIND.NONE;

        /// <summary>
        /// .. 카드의 등급
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardGrade>k__BackingField"), DisplayAs("등급"), Tooltip("카드의 등급")]
        public TM_CARD_GRADE CardGrade { get; private set; } = TM_CARD_GRADE.NORMAL;

        /// <summary>
        /// .. 카드의 그룹
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardGroup>k__BackingField"), DisplayAs("그룹"), Tooltip("카드의 그룹")]
        public TM_CARD_GROUP CardGroup { get; private set; } = TM_CARD_GROUP.COMMON;

        [field: SerializeField, SpritePreview(128f)] public Sprite CardImage { get; private set; } = null;

        [field: SerializeField, FormerlySerializedAs("<IsCustomDescription>k__BackingField"), DisplayAs("커스텀 설명"), Tooltip("체크 시 기본 설명이 나오지 않습니다")]
        public string IsCustomDescription { get; private set; }

        [field: SerializeField] public LocalizedStringOption CardName { get; private set; }

        [SerializeReference, FormerlySerializedAs("_effectCreators"), DisplayAs("카드 효과"), Tooltip("카드 효과 리스트"), SerializeReferenceDropdown]
        private List<ITMEffectCreator> _effectCreators = new();

        [SerializeReference, FormerlySerializedAs("_addtionalConditions"), DisplayAs("추가 조건"), Tooltip("카드 추가 조건 리스트"), SerializeReferenceDropdown]
        private List<ITMCardAddtionalCondition> _addtionalConditions = new();

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
                _addtionalConditions.All(additionalCondition => additionalCondition?.AdditionalCondition ?? false);
        }
    }
}