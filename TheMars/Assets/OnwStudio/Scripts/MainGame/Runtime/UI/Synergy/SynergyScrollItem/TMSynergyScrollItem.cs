using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using Onw.Helper;
using TM.Synergy;
using TM.Synergy.Effect;

namespace TM.Runtime.UI
{
    public sealed class TMSynergyScrollItem : MonoBehaviour
    {
        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _synergyIcon;
        
        [Header("Text")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _buildingCountText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyNameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyLevelText;

        public void SetView(TMSynergy synergy)
        {
            _synergyIcon.sprite = synergy.SynergyData.Icon;
            _buildingCountText.text = synergy.BuildingCount.ToString();
            _synergyNameText.text = synergy.SynergyName;

            TMSynergyEffect selectLastEffect = synergy
                .SynergyEffects
                .Where(synergyEffect => synergyEffect.TargetBuildingCount <= synergy.BuildingCount)
                .OrderBy(targetCount => targetCount)
                .LastOrDefault();

            _synergyLevelText.text = string.Join(RichTextFormatter.Colorize(" > ", Color.gray), synergy
                .SynergyEffects
                .OrderBy(effect => effect.TargetBuildingCount)
                .Select(synergyEffect => synergyEffect != selectLastEffect ?
                    RichTextFormatter.Colorize(synergyEffect.TargetBuildingCount.ToString(), Color.gray) :
                    synergyEffect.TargetBuildingCount.ToString()));
        }
    }
}
