using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using Onw.Localization;
using TM.Card.Effect;
using TM.Card.Effect.Creator;

namespace TM.Card
{
    public sealed partial class TMCardData : ScriptableObject
    {
        public event LocalizedString.ChangeHandler OnChangedName
        {
            add => _localizedCardName.StringChanged += value;
            remove => _localizedCardName.StringChanged -= value;
        }
        
        [field: SerializeField, DisplayAs("메인 코스트")]
        public TMCardMainCost MainCost { get; private set; }
        
        [field: SerializeField, DisplayAs("카드 이미지"), Tooltip("카드의 대표 이미지"), SpritePreview]
        public Sprite CardImage { get; private set; } = null;

        public string CardName => _localizedCardName.TryGetLocalizedString(out string cardName) ? cardName : "";

        public IReadOnlyList<TMCardSubCost> CardCosts => _cardCosts;
        
        [SerializeField, LocalizedString(tableName: "CardName")] private LocalizedString _localizedCardName;
        
        [SerializeReference, SerializeReferenceDropdown, DisplayAs("카드 효과"), Tooltip("카드 효과")]
        private TMCardEffectCreator _cardEffectCreator = null;

        [field: SerializeField, DisplayAs("서브 코스트"), Tooltip("서브 코스트")]
        private List<TMCardSubCost> _cardCosts;

        public TMCardKind Kind => CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>() ? TMCardKind.CONSTRUCTION : TMCardKind.EFFECT;

        public ITMCardEffect CreateCardEffect()
        {
            return _cardEffectCreator?.CreateEffect();
        }

        public bool CheckTypeEffectCreator<TEffectCreator>() where TEffectCreator : TMCardEffectCreator => _cardEffectCreator is TEffectCreator;
    }
}