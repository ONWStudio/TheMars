using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;
using Onw.UI;
using TMCard.Manager;
using TMCard.AddtionalCondition;
using TMCard.SpecialEffect;
using TMCard.Effect;
using TMCard.Effect.Resource;

namespace TMCard.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardDescriptor : MonoBehaviour
    {
        public IReadOnlyList<RectTransform> Descriptors => _descriptors;

        [SerializeField, ReadOnly] private List<RectTransform> _descriptors = new();

        [SerializeField, InitializeRequireComponent] private RectTransform _descriptionArea = null;
        [SerializeField, InitializeRequireComponent] private VerticalLayoutGroup _verticalLayoutGroup = null;

        public void SetDescription(TMCardData cardData)
        {
            foreach (ITMCardSpecialEffect specialEffect in cardData.SpecialEffects)
            {
                StringBuilder stringBuilder = new();
                stringBuilder.Append(TMSpecialEffectNameManager.Instance.GetName(specialEffect.GetType().Name));

                TextMeshProUGUI tmpText = UIHelper
                    .GetNewUIObject(transform, specialEffect.GetType().Name)
                    .gameObject
                    .AddComponent<TextMeshProUGUI>();

                tmpText.text = stringBuilder.ToString();
            }

            foreach (ITMCardEffect specialEffect in cardData.CardEffects)
            {
                if (specialEffect is TMCardResourceEffect resourceEffect)
                {
                    StringBuilder stringBuilder = new();
                    stringBuilder.Append(resourceEffect.Amount > 0 ? "+" : "-");
                    stringBuilder.Append(resourceEffect switch
                    {
                        MarsLithumEffect marsLithumEffect => "마르스리튬",
                        TeraResourceEffect teraResource => "테라",
                        _ => "[Null]"
                    });

                    TextMeshProUGUI tmpText = UIHelper
                        .GetNewUIObject(transform, specialEffect.GetType().Name)
                        .gameObject
                        .AddComponent<TextMeshProUGUI>();

                    tmpText.text = stringBuilder.ToString();
                }
            }
        }
    }
}
