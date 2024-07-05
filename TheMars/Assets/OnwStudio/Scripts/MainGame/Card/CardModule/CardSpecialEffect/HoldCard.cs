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
    [SerializeReferenceDropdownName("보유")]
    public sealed class HoldCard : ICardSpecialEffect
    {
        [SerializeField, DisplayAs("발동 트리거 카드"), Tooltip("보유 효과가 발동할때 참조할 카드 ID")]
        private TMCardData _friendlyCard = null;

        [SerializeReference, DisplayAs("보유 효과"), Tooltip("보유 효과"), SerializeReferenceDropdown]
        private List<ITMCardEffect> _holdEffects = new();

        public void ApplyEffect(TMCardController cardController)
        {
            TMCardGameManager.Instance.OnUsedCard.AddListener(onHoldEffectByFriendlyCard);

            // .. HoldCard카드는 게임상에서 스크립터블 오브젝트를 통해 참조하므로 클래스 멤버 함수로 선언하면 cardController가 마지막에 ApplyEffect를 호출했던 카드 컨트롤러로 고정된다
            // 그러므로 로컬함수로 ApplyEffect를 호출할때마다 onHoldEffectByFriendlyCard 인스턴스를 생성해야한다
            // .. 로컬함수는 Action<TMCardController> onHoldEffectByFriendlyCard와 같다 딜리게이트 인스턴스를 새로 생성하는 것과 같으므로 ..
            void onHoldEffectByFriendlyCard(TMCardController usedCard)
            {
                if (!cardController)
                {
                    TMCardGameManager.Instance.OnUsedCard.RemoveListener(onHoldEffectByFriendlyCard);
                    return;
                }

                if (cardController.OnField && _friendlyCard.Guid == usedCard.CardData.Guid)
                {
                    _holdEffects.ForEach(holdEffect => holdEffect?.OnEffect(cardController.CardData));
                    Debug.Log("보유 효과 발동");
                }
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
                Debug.LogWarning("보유 효과의 카드는 자기 자신을 트리거 카드로 지정할 수 없습니다");
                _friendlyCard = null;
            }
        }
#endif
    }
}
