using Onw.Attribute;
using Onw.Extensions;
using Onw.ServiceLocator;
using TMCard;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
namespace TM.UI
{
    public sealed class PlayerViewer : MonoBehaviour
    {
        [FormerlySerializedAs("_teraText")]
        [FormerlySerializedAs("teraText")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _creditText;
        
        [FormerlySerializedAs("_marsLithumText")]
        [FormerlySerializedAs("marsLithumText")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _marsLithiumText;

        private void Start()
        {
            this
                .ObserveEveryValueChanged(playerViewer => ServiceLocator<PlayerManager>.TryGetService(out PlayerManager playerManager) ? playerManager : null)
                .Where(playerManager => playerManager)
                .Take(1)
                .Subscribe(playerManager =>
                {
                    playerManager.ObserveInformation(
                        player => player.Credit,
                        credit => _creditText.text = $"<sprite={(int)TMRequiredResource.CREDIT}> {credit}");

                    playerManager.ObserveInformation(
                        player => player.MarsLithium,
                        marsLithium => _marsLithiumText.text = $"<sprite={(int)TMRequiredResource.MARS_LITHIUM}> {marsLithium}");
                });
        }
    }
}