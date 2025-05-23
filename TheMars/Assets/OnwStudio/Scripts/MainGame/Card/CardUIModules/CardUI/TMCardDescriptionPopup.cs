
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using TMPro;
using Onw.UI;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Scope;
using TM.Card.Runtime;
using UnityEngine.Events;
using Onw.UI.Components;
using UnityEngine.EventSystems;

namespace TM.UI
{
    public class TMCardDescriptionPopup : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _cardNameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _cardDescriptionText;

        [SerializeField, InitializeRequireComponent] private RectTransform _rectTransform;
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;

        [SerializeField, LocalizedString(tableName: "TM_UI", entryKey: "PaymentHeader")] private LocalizedString _paymentHeader;
        [SerializeField, LocalizedString(tableName: "TM_UI", entryKey: "EffectHeader")] private LocalizedString _effectHeader;

        public TMCardModel _card = null;

        [SerializeField] private Vector2 _multiplyOffset = Vector2.zero;
        
        private bool _canShow = true;
        private bool _isReload;

        public void OnAddedCardForCollect(TMCardModel card)
        {
            card.OnUnSafePointerEnterEvent += SetUI;
            card.OnUnSafePointerExitEvent += onPointerExitEvent;
        }

        public void OnRemovedCardForCollect(TMCardModel card)
        {
            card.OnUnSafePointerEnterEvent -= SetUI;
            card.OnUnSafePointerExitEvent -= onPointerExitEvent;

            if (_card == card)
            {
                removeListenerToLocalizedString(card);
                _card = null;
                _cardDescriptionText.text = string.Empty;
                _cardNameText.text = string.Empty;
                _canvas.enabled = false;
            }
        }

        public void OnAddedCard(TMCardModel card)
        {
            card.OnSafePointerEnterEvent += SetUI;
            card.OnSafePointerExitEvent += onPointerExitEvent;
        }

        public void OnRemovedCard(TMCardModel card)
        {
            card.OnSafePointerEnterEvent -= SetUI;
            card.OnSafePointerExitEvent -= onPointerExitEvent;

            if (_card == card)
            {
                removeListenerToLocalizedString(card);
                _card = null;
                _cardDescriptionText.text = string.Empty;
                _cardNameText.text = string.Empty;
                _canvas.enabled = false;
            }
        }

        private void addListenerToLocalizedString(TMCardModel card)
        {
            card.CardData.Value.OnChangedName += onChangedName;
            card.MainCost.CostDescription.StringChanged += onChangedLocalizedString;
            card.SubCosts.ForEach(cost => cost.CostDescription.StringChanged += onChangedLocalizedString);
            card.CardEffect.LocalizedDescription.StringChanged += onChangedLocalizedString;
        }

        private void removeListenerToLocalizedString(TMCardModel card)
        {
            card.CardData.Value.OnChangedName -= onChangedName;
            card.MainCost.CostDescription.StringChanged -= onChangedLocalizedString;
            card.SubCosts.ForEach(cost => cost.CostDescription.StringChanged -= onChangedLocalizedString);
            card.CardEffect.LocalizedDescription.StringChanged -= onChangedLocalizedString;
        }

        private void onPointerExitEvent(TMCardModel card)
        {
            if (!_card || _card != card) return;

            removeListenerToLocalizedString(_card);
            _card = null;
            _canvas.enabled = false;
            _cardDescriptionText.text = string.Empty;
            _cardNameText.text = string.Empty;
        }

        public void SetUI(TMCardModel card)
        {
            if (!card || !_canShow) return;

            if (card.IsDragging.Value)
            {
                onDragBeginCard(card);
                return; // 여기에 return을 추가합니다.
            }

            if (_card)
            {
                removeListenerToLocalizedString(_card);
            }

            _card = card;
            addListenerToLocalizedString(_card);

            _card.OnDragBeginCard -= onDragBeginCard;
            _card.OnDragBeginCard += onDragBeginCard;

            positionPopupAtCardTopRight();
        }

        private void positionPopupAtCardTopRight()
        {
            RectTransform cardRect = _card.RectTransform;
            Vector3[] cardCorners = cardRect.GetWorldCorners();
            Vector2 cardSize = cardRect.GetWorldRectSize();

            Vector3 cardTopRightWorldPosition = new(
                cardCorners[2].x + cardSize.x * _multiplyOffset.x,
                cardCorners[2].y + cardSize.y * _multiplyOffset.y,
                _rectTransform.GetPositionZ());

            _rectTransform.pivot = new(0f, 0f);
            _rectTransform.position = cardTopRightWorldPosition;

            _canvas.enabled = true;
        }

        private void onChangedLocalizedString(string _)
        {
            if (_isReload) return;

            _isReload = true;
            StartCoroutine(iEReloadDescription(_card));
        }

        // .. TODO : 카드 설명 출력
        private IEnumerator iEReloadDescription(TMCardModel card)
        {
            yield return null;

            using (StringBuilderPoolScope scope = new())
            {
                StringBuilder descriptionBuilder = scope.Get();
                descriptionBuilder.AppendLine(_paymentHeader.GetLocalizedString());
                descriptionBuilder.AppendLine("");
                descriptionBuilder.AppendLine(card.MainCost.CostDescription.GetLocalizedString());
                card.SubCosts.ForEach(cost => descriptionBuilder.AppendLine(cost.CostDescription.GetLocalizedString()));
                descriptionBuilder.AppendLine("");
                descriptionBuilder.AppendLine(_effectHeader.GetLocalizedString());
                descriptionBuilder.AppendLine("");
                descriptionBuilder.AppendLine(card.CardEffect.LocalizedDescription.GetLocalizedString());
                _cardDescriptionText.text = descriptionBuilder.ToString();
            }

            _isReload = false;
        }
        private void onChangedName(string cardName)
        {
            _cardNameText.text = cardName;
        }

        private void onDragBeginCard(TMCardModel card)
        {
            card.OnDragBeginCard -= onDragBeginCard;
            card.OnDragEndCard += onDragEndCard;

            _canShow = false;
            _canvas.enabled = false;
            removeListenerToLocalizedString(card);
            _card = null;
            _cardDescriptionText.text = string.Empty;
            _cardNameText.text = string.Empty;
        }

        private void onDragEndCard(TMCardModel card)
        {
            card.OnDragEndCard -= onDragEndCard;
            _canShow = true;
        }
    }
}