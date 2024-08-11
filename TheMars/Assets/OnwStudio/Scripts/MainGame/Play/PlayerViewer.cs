using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;

namespace TM.UI
{
    public sealed class PlayerViewer : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _teraText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _marsLithumText;
        
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