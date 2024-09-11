using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using Onw.Attribute;
using Onw.Localization;
using TM.Card.Effect;
using TM.Card.Effect.Creator;

namespace TM.Card
{
    public sealed partial class TMCardData : ScriptableObject
    {
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

        public IReadOnlyList<TMCardCost> CardCosts => _cardCosts;
        
        [SerializeField] private LocalizedString _localizedCardName;
        
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("카드 효과"), Tooltip("카드 효과")]
        private ITMCardEffectCreator _cardEffectCreator = null;

        [field: SerializeField, DisplayAs("코스트"), Tooltip("소모할 재화들 입니다")]
        private List<TMCardCost> _cardCosts;
        
        public ITMCardEffect CreateCardEffect()
        {
            return _cardEffectCreator?.CreateEffect();
        }
    }
}