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
using Onw.Localization;
using Onw.Coroutine;
using System.Linq;
using TM.Event.Effect;
using TM.Usage;
using UnityEngine.Localization;
using System;

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

        [Header("Triggers")]
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _topButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerEnterTrigger _bottomButtonEnterTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _topButtonExitTrigger;
        [SerializeField, SelectableSerializeField] private PointerExitTrigger _bottomButtonExitTrigger;

        private string _description = string.Empty;
        private string _topEffectDescription = string.Empty;
        private string _bottomEffectDescription = string.Empty;
        private TMEventRunner _eventRunner = null;

        private readonly LocalizedString _paymentDescriptionHeader = new("TM_UI", "EventPaymentHeader");
        private readonly LocalizedString _eventEffectHeader = new("TM_UI", "EventEffectHeader");

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
                buildDescription(_eventRunner.TopEffects, _eventRunner.TopUsages, buildedText => _topEffectDescription = buildedText);
            }
            
            _bottomButton.SetActiveGameObject(_eventRunner.EventData.HasBottomEvent);
            if (_eventRunner.EventData.HasBottomEvent)
            {
                _bottomButtonTextEvent.StringReference = _eventRunner.EventData.BottomButtonTextEvent;
                buildDescription(_eventRunner.BottomEffects, _eventRunner.BottomUsages, buildedText => _bottomEffectDescription = buildedText);
            }

            void buildDescription(IReadOnlyList<ITMEventEffect> effects, IReadOnlyList<ITMUsage> usages, Action<string> stringAction)
            {
                bool isReload = false;
                effects.ForEach(effect => effect.EffectDescription.StringChanged += onUpdateString);
                usages.ForEach(usage => usage.UsageLocalizedString.StringChanged += onUpdateString);

                void onUpdateString(string text)
                {
                    if (isReload) return;

                    isReload = true;
                    this.DoCallWaitForOneFrame(() => // .. 1프레임 대기 후 호출
                    {
                        isReload = false;
                        stringAction?.Invoke((usages.Count > 0 ?
                                _paymentDescriptionHeader.GetLocalizedString() + 
                                "\n" + 
                                string.Join("\n", effects.Select(effect => effect.EffectDescription.GetLocalizedString())) : "") +
                           (effects.Count > 0 ? 
                                _eventEffectHeader.GetLocalizedString() + 
                                "\n" +
                                string.Join("\n", usages.Select(usage => usage.UsageLocalizedString.GetLocalizedString())) : ""));
                    });
                }
            }
        }
    }
}