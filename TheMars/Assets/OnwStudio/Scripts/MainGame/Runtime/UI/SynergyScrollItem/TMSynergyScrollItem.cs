using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
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
            _synergyNameText.text = string.Join(">", synergy
                .SynergyEffects
                .Select(synergyEffect => synergyEffect.TargetBuildingCount)
                .OrderBy(targetCount => targetCount));
        }
    }
}
