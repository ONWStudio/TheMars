using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;
using Onw.Helper;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using TM.Synergy;
using TM.Synergy.Effect;
// ReSharper disable PossibleNullReferenceException

namespace TM.UI
{
    public sealed class TMSynergyScrollItem : MonoBehaviour, IReturnHandler
    {
        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _synergyIcon;
        
        [Header("Text")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _buildingCountText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyNameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyLevelText;

        private LocalizedString _synergyNameLocalizedName;
        
        public void SetView(TMSynergy synergy)
        {
            _synergyIcon.sprite = synergy.SynergyData.Icon;
            _buildingCountText.text = synergy.BuildingCount.ToString();
            _synergyNameLocalizedName = synergy.LocalizedSynergyName;
            _synergyNameLocalizedName.StringChanged += onChangedSynergyName;

            TMSynergyEffect[] sortedEffects = synergy
                .SynergyEffects
                .OrderBy(effect => effect.TargetBuildingCount)
                .ToArray();

            StringBuilder synergyLevelBuilder = new();
            
            for (int i = 0; i < sortedEffects.Length; i++)
            {
                TMSynergyEffect effect = sortedEffects[i];
                bool isEnabled = effect.TargetBuildingCount <= synergy.BuildingCount;
                if (isEnabled)
                {
                    synergyLevelBuilder.Append(effect.TargetBuildingCount.ToString());

                    if (i < synergy.SynergyEffects.Count - 1)
                    {
                        synergyLevelBuilder.Append(" > ");
                    }
                }
                else
                {
                    synergyLevelBuilder.Append(RichTextFormatter.Colorize(effect.TargetBuildingCount.ToString(), Color.gray));

                    if (i < synergy.SynergyEffects.Count - 1)
                    {
                        synergyLevelBuilder.Append(RichTextFormatter.Colorize(" > ", Color.gray));
                    }
                }

                _synergyLevelText.text = synergyLevelBuilder.ToString();
            }
        }

        private void onChangedSynergyName(string synergyName)
        {
            _synergyNameText.text = synergyName;
        }
        
        public void OnReturnToPool()
        {
            _synergyNameLocalizedName.StringChanged -= onChangedSynergyName;
        }

        private void OnDestroy()
        {
            _synergyNameLocalizedName.StringChanged -= onChangedSynergyName;
        }
    }
}
