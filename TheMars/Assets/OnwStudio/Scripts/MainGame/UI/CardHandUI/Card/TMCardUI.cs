using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UITools.UITool;

[DisallowMultipleComponent]
public sealed class TMCardUI : MonoBehaviour
{
    public EventReceiver EventReceiver { get; private set; } = null;

    private CardInputHandler _cardEventHandler = null;
    private RectTransform _rectTransform = null;
    private Image _raycastingImage = null;
    private Image _cardImage = null;
    private SmoothMoveVector2 _smoothMove = null;

    private void Awake()
    {
        Debug.Log("Awake");

        _raycastingImage = gameObject.AddComponent<Image>();
        _cardEventHandler = gameObject.AddComponent<CardInputHandler>();
        EventReceiver = gameObject.AddComponent<EventReceiver>();
        _cardImage = new GameObject("CardImage").AddComponent<Image>();
        _smoothMove = _cardImage.gameObject.AddComponent<SmoothMoveVector2>();
        _rectTransform = transform as RectTransform;
    }

    private void Start()
    {
        _raycastingImage.color = new(255f, 255f, 255f, 0f);

        _cardImage.transform.SetParent(transform, false);
        _cardImage.raycastTarget = false;
        _cardImage.transform.localPosition = Vector3.zero;

        _smoothMove.IsLocal = true;

        _cardEventHandler.AddListenerPointerEnterAction(pointerEventData
            => _smoothMove.TargetPosition = 0.5f * _rectTransform.rect.height * new Vector3(0f, 1f, 0f));

        _cardEventHandler.AddListenerPointerExitAction(pointerEventData
            => _smoothMove.TargetPosition = Vector2.zero);

        _cardEventHandler.AddListenerPointerClickAction(pointerEventData
            => { });
    }

    public void SetCardUI(Transform parent)
    {
        transform.SetParent(parent, false);
    }
}