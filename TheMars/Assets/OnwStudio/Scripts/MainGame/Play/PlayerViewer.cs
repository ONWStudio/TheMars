using Onw.Attribute;
using Onw.Extensions;
using TMCard;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
namespace TM.UI
{
    public sealed class PlayerViewer : MonoBehaviour
    {
        [FormerlySerializedAs("teraText")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _teraText;
        [FormerlySerializedAs("marsLithumText")]
        [SerializeField, SelectableSerializeField]
        private TextMeshProUGUI _marsLithumText;

        private void Start()
        {
            UniRxObserver.ObserveInfomation(
                this,
                selector => PlayerManager.Instance.Tera,
                tera => _teraText.text = $"<sprite={(int)TMRequiredResource.TERA}> {tera}");

            UniRxObserver.ObserveInfomation(
                this,
                selector => PlayerManager.Instance.MarsLithium,
                marsLithum => _marsLithumText.text = $"<sprite={(int)TMRequiredResource.MARS_LITHIUM}> {marsLithum}");
        }
    }
}