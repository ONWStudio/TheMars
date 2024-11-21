using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using Onw.Helper;
using Onw.Manager.ObjectPool;
using TM.Synergy;
using TM.Synergy.Effect;
using UnityEngine.Localization;
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

        private LocalizedString _synergyNameLocalizedName = null;
        
        public void SetView(TMSynergy synergy)
        {
            _synergyIcon.sprite = synergy.SynergyData.Icon;
            _buildingCountText.text = synergy.BuildingCount.ToString();
            _synergyNameLocalizedName = synergy.LocalizedSynergyName;
            _synergyNameLocalizedName.StringChanged += onChangedSynergyName;

            TMSynergyEffect selectLastEffect = synergy
                .SynergyEffects
                .Where(synergyEffect => synergyEffect.TargetBuildingCount <= synergy.BuildingCount)
                .OrderBy(targetCount => targetCount) // .. GC 발생
                .LastOrDefault();

            StringBuilder synergyLevelBuilder = new();
            
            for (int i = 0; i < synergy.SynergyEffects.Count; i++)
            {
                TMSynergyEffect effect = synergy.SynergyEffects[i];
                bool isEnabled = effect == selectLastEffect;
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
                    synergyLevelBuilder.Append(RichTextFormatter.Colorize(" > ", Color.gray));
                    if (i < synergy.SynergyEffects.Count - 1)
                    {
                        synergyLevelBuilder.Append(RichTextFormatter.Colorize(effect.TargetBuildingCount.ToString(), Color.gray));
                    }
                }
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
