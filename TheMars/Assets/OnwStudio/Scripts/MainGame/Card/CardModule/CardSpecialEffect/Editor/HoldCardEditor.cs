#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;

namespace TMCard.SpecialEffect
{
    public sealed partial class HoldCard : ITMCardSpecialEffect
    {
        [OnValueChangedByMethod(nameof(_friendlyCard))]
        private void onChangedFriendlyCard()
        {
            Debug.Log("changed friendlyCard");

            TMCardData ownerCard = AssetDatabase
                .FindAssets($"t:{typeof(TMCardData).Name}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<TMCardData>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(cardData => cardData.SpecialEffects.Any(specialEffect => specialEffect == this));

            if (ownerCard && ownerCard == _friendlyCard)
            {
                Debug.LogWarning("보유 효과의 카드는 자기 자신을 트리거 카드로 지정할 수 없습니다");
                _friendlyCard = null;
            }
        }
    }
}
#endif
