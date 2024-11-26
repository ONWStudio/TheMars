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
using UnityEngine.Events;
using UnityEngine.EventSystems;
// ReSharper disable PossibleNullReferenceException

namespace TM.UI
{
    public sealed class TMSynergyScrollItem : MonoBehaviour, IReturnHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event UnityAction OnPointerEnterEvent
        {
            add => _onPointerEnterEvent.AddListener(value);
            remove => _onPointerExitEvent.RemoveListener(value);
        }
        
        public event UnityAction OnPointerExitEvent
        {
            add => _onPointerExitEvent.AddListener(value);
            remove => _onPointerExitEvent.RemoveListener(value);
        }
        
        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _synergyIcon;

        [Header("Text")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _buildingCountText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyNameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _synergyLevelText;

        [SerializeField] private UnityEvent _onPointerEnterEvent = new();
        [SerializeField] private UnityEvent _onPointerExitEvent = new();

        private LocalizedString _synergyNameLocalizedName;

        private void OnDestroy()
        {
            OnReturnToPool();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _onPointerEnterEvent.Invoke();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _onPointerExitEvent.Invoke();
        }
        
        public void Initialize(TMSynergy synergy)
        {
            _synergyNameLocalizedName = synergy.LocalizedSynergyName;
            _synergyNameLocalizedName.StringChanged += onChangedSynergyName;
        }

        public void SetView(TMSynergy synergy)
        {
            _synergyIcon.sprite = synergy.SynergyData.Icon;
            _buildingCountText.text = synergy.BuildingCount.ToString();

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
                    if (i > 0)
                    {
                        synergyLevelBuilder.Append(" > ");
                    }
                    
                    synergyLevelBuilder.Append(effect.TargetBuildingCount.ToString());
                }
                else
                {
                    if (i > 0)
                    {
                        synergyLevelBuilder.Append(RichTextFormatter.Colorize(" > ", Color.gray));
                    }
                    
                    synergyLevelBuilder.Append(RichTextFormatter.Colorize(effect.TargetBuildingCount.ToString(), Color.gray));
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
            if (_synergyNameLocalizedName is not null)
            {
                _synergyNameLocalizedName.StringChanged -= onChangedSynergyName;
                _synergyNameLocalizedName = null;
            }

            _onPointerEnterEvent.RemoveAllListeners();
            _onPointerExitEvent.RemoveAllListeners();
        }
    }
}