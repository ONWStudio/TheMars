using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace TMCard
{
    using UI;

    /// <summary>
    /// .. 보유
    /// </summary>
    [System.Serializable, SerializeReferenceDropdownName("보유")]
    public sealed class HoldCard : ICardSpecialEffect
    {
        [SerializeField, DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID")]
        private TMCardData _friendlyCard = null;

        [field: SerializeField, DisplayAs("보유 효과"), Tooltip("보유 효과")]
        private List<ITMCardEffect> _holdEffects = new();

        private TMCardController _cardController = null;

        public void ApplyEffect(TMCardController cardController)
        {
            _cardController = cardController;

            TMCardGameManager.Instance.OnUsedCard.AddListener(onHoldEffectByFriendlyCard);
        }

        private void onHoldEffectByFriendlyCard(TMCardController friendlyCard)
        {
            if (!_cardController) // .. 카드가 파괴되었을 경우 콜백메서드는 호출 될 수 있으므로
            {
                TMCardGameManager.Instance.OnUsedCard.RemoveListener(onHoldEffectByFriendlyCard);
                return;
            }

            if (_cardController.OnCard && _friendlyCard.Guid == friendlyCard.CardData.Guid)
            {
                _holdEffects.ForEach(holdEffect => holdEffect?.OnEffect(_cardController.CardData));
                Debug.Log("보유 효과 발동");
            }
        }

#if UNITY_EDITOR
        [OnValueChangedByMethod(nameof(_friendlyCard))]
        private void onChangedFriendlyCard()
        {
            Debug.Log("changed friendlyCard");

            TMCardData ownerCard = UnityEditor
                .AssetDatabase
                .FindAssets($"t:{typeof(TMCardData).Name}")
                .Select(guid => UnityEditor.AssetDatabase.LoadAssetAtPath<TMCardData>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(cardData => cardData.SpecialEffects.Any(specialEffect => specialEffect == this));

            if (ownerCard && ownerCard == _friendlyCard)
            {
                _friendlyCard = null;
            }
        }
#endif
    }
}
