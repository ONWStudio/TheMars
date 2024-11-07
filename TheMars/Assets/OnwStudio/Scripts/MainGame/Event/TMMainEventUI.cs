using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using TMPro;
using Onw.Attribute;
using Onw.Extensions;
using Onw.UI.Components;
using TM.Event;

namespace TM.Runtime.UI
{
    public sealed class TMMainEventUI : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField, SelectableSerializeField] private Button _topButton;
        [SerializeField, SelectableSerializeField] private Button _bottomButton;

        [Header("Image")]
        [SerializeField, SelectableSerializeField] private Image _eventImage;

        [Header("Text")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _topButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _bottomButtonText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _titleText;

        [Header("Localization Event")]
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _descriptionTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _topButtonTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _bottomButtonTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _titleTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _topEffectTextEvent;
        [SerializeField, SelectableSerializeField] private LocalizeStringEvent _bottomEffectTextEvent;

        [Header("Triggers")]
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _topButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _bottomButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _topButtonExitTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _bottomButtonExitTrigger;

        private string _description = string.Empty;
        private string _topEffectDescription = string.Empty;
        private string _bottomEffectDescription = string.Empty;
        private TMEventRunner _eventRunner = null;

        private void Start()
        {
            _topButton.onClick.AddListener(onClickTopButton);
            _bottomButton.onClick.AddListener(onClickBottomButton);

            _topButtonEnterTrigger.OnPointerEnterEvent += onPointerEnterByTop;
            _bottomButtonEnterTrigger.OnPointerEnterEvent += onPointerEnterByBottom;
            _topButtonExitTrigger.OnPointerExitEvent += onPointerExitByTop;
            _bottomButtonExitTrigger.OnPointerExitEvent += onPointerExitByBottom;

            _descriptionTextEvent.OnUpdateString.AddListener(description => _descriptionText.text = _description = description);
            _topButtonTextEvent.OnUpdateString.AddListener(topText => _topButtonText.text = topText);
            _bottomButtonTextEvent.OnUpdateString.AddListener(bottomText => _bottomButtonText.text = bottomText);
            _titleTextEvent.OnUpdateString.AddListener(titleText => _titleText.text = titleText);
            _topEffectTextEvent.OnUpdateString.AddListener(topEffectText => _topEffectDescription = topEffectText);
            _bottomEffectTextEvent.OnUpdateString.AddListener(bottomEffectText => _bottomEffectDescription = bottomEffectText);
        }

        private void onPointerEnterByTop(PointerEventData eventData)
        {
            _descriptionText.text = _topEffectDescription;
        }

        private void onPointerEnterByBottom(PointerEventData eventData)
        {
            _descriptionText.text = _bottomEffectDescription;
        }

        private void onPointerExitByTop(PointerEventData eventData)
        {
            _descriptionText.text = _description;
        }

        private void onPointerExitByBottom(PointerEventData eventData)
        {
            _descriptionText.text = _description;
        }

        private void onClickTopButton()
        {
            if (!_eventRunner?.CanFireTop ?? true) return;
            onEffect(TMEventChoice.TOP);
        }

        private void onClickBottomButton()
        {
            if (!_eventRunner?.CanFireBottom ?? true) return;
            onEffect(TMEventChoice.BOTTOM);
        }

        private void onEffect(TMEventChoice choice)
        {
            _eventRunner.InvokeEvent(choice);
            this.SetActiveGameObject(false);
            resetField();
        }

        private void resetField()
        {
            _description = string.Empty;
            _topEffectDescription = string.Empty;
            _bottomEffectDescription = string.Empty;
            _eventRunner = null;
            _eventImage.sprite = null;
            _descriptionTextEvent.StringReference = null;
            _topButtonTextEvent.StringReference = null;
            _bottomButtonTextEvent.StringReference = null;
            _titleTextEvent.StringReference = null;
            _topEffectTextEvent.StringReference = null;
            _bottomEffectTextEvent.StringReference = null;
        }

        public void OnTriggerMainEvent(TMEventRunner mainEventRunner)
        {
            this.SetActiveGameObject(true);

            _eventRunner = mainEventRunner;
            _eventImage.sprite = _eventRunner.EventData.EventImage;
            _titleTextEvent.StringReference = _eventRunner.EventData.TitleTextEvent;
            _descriptionTextEvent.StringReference = _eventRunner.EventData.DescriptionTextEvent;

            _topButton.SetActiveGameObject(_eventRunner.EventData.HasTopEvent);
            if (_eventRunner.EventData.HasTopEvent)
            {
                _topButtonTextEvent.StringReference = _eventRunner.EventData.TopButtonTextEvent;
                _topEffectTextEvent.StringReference = _eventRunner.EventData.TopEffectTextEvent;
                if (_eventRunner.EventData.TopEffectLocalizedArguments is not null)
                {
                    _topEffectTextEvent.StringReference.Arguments = new object[]
                    {
                        _eventRunner.EventData.TopEffectLocalizedArguments
                    };
                }
            }
            
            _bottomButton.SetActiveGameObject(_eventRunner.EventData.HasBottomEvent);
            if (_eventRunner.EventData.HasBottomEvent)
            {
                _bottomButtonTextEvent.StringReference = _eventRunner.EventData.BottomButtonTextEvent;
                _bottomEffectTextEvent.StringReference = _eventRunner.EventData.BottomEffectTextEvent;
                if (_eventRunner.EventData.BottomEffectLocalizedArguments is not null)
                {
                    _bottomEffectTextEvent.StringReference.Arguments = new object[]
                    {
                        _eventRunner.EventData.BottomEffectLocalizedArguments
                    };
                }
            }
        }
    }
}