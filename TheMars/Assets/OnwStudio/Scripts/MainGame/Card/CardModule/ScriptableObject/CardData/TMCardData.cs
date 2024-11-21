using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Onw.Attribute;
using Onw.Localization;
using TM.Card.Effect;
using TM.Card.Effect.Creator;
using TM.Cost.Creator;
using TM.Cost;
using System.Linq;
using Onw.Extensions;

namespace TM.Card
{
    public sealed partial class TMCardData : ScriptableObject
    {
        public event LocalizedString.ChangeHandler OnChangedName
        {
            add => _localizedCardName.StringChanged += value;
            remove => _localizedCardName.StringChanged -= value;
        }

        [SerializeField, LocalizedString(tableName: "CardName")] private LocalizedString _localizedCardName;

        [SerializeReference, SerializeReferenceDropdown, DisplayAs("카드 효과"), Tooltip("카드 효과")]
        private TMCardEffectCreator _cardEffectCreator = null;

        [SerializeReference, SerializeReferenceDropdown, DisplayAs("메인 코스트"), Tooltip("메인 코스트")] 
        private ITMResourceCostCreator _mainCost;

        [SerializeReference, SerializeReferenceDropdown, DisplayAs("서브 코스트"), Tooltip("서브 코스트")]
        private List<ITMResourceCostCreator> _subCosts;

        [field: SerializeField, ReadOnly] 
        public string ID { get; private set; } = Guid.NewGuid().ToString();
        
        [field: SerializeField, DisplayAs("카드 이미지"), Tooltip("카드의 대표 이미지"), SpritePreview]
        public Sprite CardImage { get; private set; } = null;

        [field: SerializeField, DisplayAs("카드 고유 키 (모든 카드의 키값은 달라야 합니다)")]
        public string CardKey { get; private set; } = string.Empty;
        public string CardName => _localizedCardName.TryGetLocalizedString(out string cardName) ? cardName : "";

        public TMCardKind Kind => CheckTypeEffectCreator<TMCardBuildingCreateEffectCreator>() ? TMCardKind.CONSTRUCTION : TMCardKind.EFFECT;

        public ITMCardEffect CreateCardEffect()
        {
            return _cardEffectCreator?.CreateEffect();
        }

        public ITMResourceCost CreateMainCost()
        {
            return _mainCost?.CreateCost() as ITMResourceCost;
        }

        public ITMResourceCost[] CreateSubCosts()
        {
            return _subCosts?
                .Where(cost => cost is not null)
                .Select(cost => cost.CreateCost())
                .OfType<ITMResourceCost>()
                .ToArray();
        }

        public bool CheckTypeEffectCreator<TEffectCreator>() where TEffectCreator : TMCardEffectCreator => _cardEffectCreator is TEffectCreator;
    }
}