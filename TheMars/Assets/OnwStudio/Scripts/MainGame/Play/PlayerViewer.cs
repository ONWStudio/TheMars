using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;
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
                tera => _teraText.text = $"<Tera> {tera}");

            UniRxObserver.ObserveInfomation(
                this,
                selector => PlayerManager.Instance.MarsLithum,
                marsLithum => _marsLithumText.text = $"<Mars Lithum> {marsLithum}");
        }
    }
}