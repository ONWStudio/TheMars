using System.Linq;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Localization;
using TM.Building;
using TMCard.AdditionalCondition;
using TMCard.Effect;
using TMCard.Runtime;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace TMCard
{
    public sealed partial class TMCardData : ScriptableObject
    {
        [field: SerializeField, DisplayAs("소모 재화 종류"), Tooltip("소모할 재화")] 
        public TMRequiredResource RequiredResource { get; private set; }
        
        [field: SerializeField, DisplayAs("소모 재화"), Tooltip("소모 재화량")]
        public int Resource { get; private set; }
            
        /// <summary>
        /// .. 카드의 종류
        /// </summary>
        [field: Space]
        [field: SerializeField, FormerlySerializedAs("<CardKind>k__BackingField"), DisplayAs("카드 종류"), Tooltip("카드의 종류")]
        public TMCardKind CardKind { get; private set; } = TMCardKind.NONE;

        /// <summary>
        /// .. 카드의 그룹
        /// </summary>
        [field: SerializeField, FormerlySerializedAs("<CardGroup>k__BackingField"), DisplayAs("그룹"), Tooltip("카드의 그룹")]
        public TMCardGroup CardGroup { get; private set; } = TMCardGroup.COMMON;

        [field: SerializeField, DisplayAs("카드 이미지"), Tooltip("카드의 대표 이미지"), SpritePreview]
        public Sprite CardImage { get; private set; } = null;

        public string CardName => _localizedCardName.TryGetLocalizedString(out string cardName) ? cardName : "";
        
        [SerializeField] private LocalizedString _localizedCardName;
        
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("카드 효과"), Tooltip("카드 효과")]
        private ITMCardEffectCreator _cardEffectCreator = null;

        public ITMCardEffect GetCardEffect()
        {
            return _cardEffectCreator?.CreateEffect();
        }
        
        public bool CanUse(int resource)
        {
            return resource >= Resource;
        }
    }
}