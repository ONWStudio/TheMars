using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using TM.Card.Runtime;
using UnityEngine.Localization;
using Onw.UI;
using Onw.Extensions;

namespace TM.UI
{
    public class TMCardDescriptionPopup : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _cardNameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _cardDescriptionText;

        [SerializeField, InitializeRequireComponent] private RectTransform _rectTransform;
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;

        public TMCardModel _card = null;

        [SerializeField] private Vector2 _multiflyOffset = Vector2.zero;
        private bool _canShow = true;

        public void OnAddedCardEvent(TMCardModel card)
        {
            card.OnSafePointerEnterEvent += SetUI;
            card.OnSafePointerExitEvent += onPointerExitEvent;
        }

        public void OnRemovedCardEvent(TMCardModel card)
        {
            card.OnSafePointerEnterEvent -= SetUI;
            card.OnSafePointerExitEvent -= onPointerExitEvent;

            if (_card == card)
            {
                _card.CardData.Value.OnChangedName -= onChangedName;
                _card.CardEffect.OnChangedDescription -= onChangedDescription;
                _card = null;
                _canvas.enabled = false;
            }
        }

        private void onPointerExitEvent(TMCardModel card)
        {
            if (!_card || _card != card) return;

            _card.CardData.Value.OnChangedName -= onChangedName;
            _card.CardEffect.OnChangedDescription -= onChangedDescription;
            _card = null;
            _canvas.enabled = false;
        }

        public void SetUI(TMCardModel card)
        {
            if (!card || !_canShow) return;

            if (card.IsDragging.Value)
            {
                onDragBeginCard(card);
            }

            if (_card)
            {
                _card.CardData.Value.OnChangedName -= onChangedName;
                _card.CardEffect.OnChangedDescription -= onChangedDescription;
            }

            _card = card;
            _card.CardData.Value.OnChangedName += onChangedName;
            _card.CardEffect.OnChangedDescription += onChangedDescription;

            _card.OnDragBeginCard -= onDragBeginCard;
            _card.OnDragBeginCard += onDragBeginCard;

            positionPopupAtCardTopRight();

            void onDragBeginCard(TMCardModel card)
            {
                card.OnDragBeginCard -= onDragBeginCard;
                card.OnDragEndCard += onDragEndCard;

                _canShow = false;
                _canvas.enabled = false;
                card.CardData.Value.OnChangedName -= onChangedName;
                card.CardEffect.OnChangedDescription -= onChangedDescription;
                _card = null;
            }

            void onDragEndCard(TMCardModel card)
            {
                card.OnDragEndCard -= onDragEndCard;
                _canShow = true;
            }
        }

        private void positionPopupAtCardTopRight()
        {
            RectTransform cardRect = _card.RectTransform;
            Vector3[] cardCorners = cardRect.GetWorldCorners();
            Vector2 cardSize = cardRect.GetWorldRectSize();

            Vector3 cardTopRightWorldPosition = new(
                cardCorners[2].x + cardSize.x * _multiflyOffset.x,
                cardCorners[2].y + cardSize.y * _multiflyOffset.y,
                _rectTransform.GetPositionZ());

            _rectTransform.pivot = new Vector2(0f, 0f);
            _rectTransform.position = cardTopRightWorldPosition;

            _canvas.enabled = true;
        }

        private void onChangedDescription(string description)
        {
            _cardDescriptionText.text = description;
        }

        private void onChangedName(string cardName)
        {
            _cardNameText.text = cardName;
        }
    }
}