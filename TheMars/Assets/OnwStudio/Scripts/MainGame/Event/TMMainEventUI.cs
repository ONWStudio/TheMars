using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;
using TM.Event;
using UnityEngine.Localization.Components;

namespace TM.Runtime.UI
{
    public sealed class TMMainEventUI : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private Button _topButton;
        [SerializeField, SelectableSerializeField] private Button _bottomButton;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _topButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _bottomButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _headerText;
        
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _descriptionTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _topButtonTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _bottomButtonTextEvent;
        
        private void Start()
        {
            TMMainEventManager.Instance.OnTriggerMainEvent += onTriggerMainEvent;
        }
        
        private void onTriggerMainEvent(TMEventData eventData)
        {
            _descriptionTextEvent.
            _descriptionTextEvent.OnUpdateString.AddListener(str => { });
        }
    }
}
