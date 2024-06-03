using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class TMCardUI : MonoBehaviour
{
    public CardMovementBase CardMovement { get; private set; } = null;

    private CardEventHandler _cardEventHandler = null;
    private Image _cardImage = null;

    private void Start()
    {

    }

    public void SetCardUI(Transform parent, CardMovementBase cardMovement)
    {
        CardMovement = cardMovement;
        transform.SetParent(parent, false);

        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        Image raycastingImage = gameObject.AddComponent<Image>();
        raycastingImage.color = new(255f, 255f, 255f, 0f);

        _cardEventHandler = gameObject.AddComponent<CardEventHandler>();
        _cardImage = new GameObject("CardImage").AddComponent<Image>();
        _cardImage.transform.SetParent(transform, false);
        _cardImage.raycastTarget = false;
        _cardImage.transform.localPosition = Vector3.zero;

        SmoothMoveVector2 smoothMove = _cardImage.gameObject.AddComponent<SmoothMoveVector2>();

        _cardEventHandler.AddListenerPointerEnterAction(()
            => smoothMove.TargetPosition = smoothMove.transform.TransformPoint(0.45f * rectTransform.rect.height * transform.up));

        _cardEventHandler.AddListenerPointerExitAction(()
            => smoothMove.TargetPosition = smoothMove.transform.TransformPoint(Vector3.zero));
    }
}