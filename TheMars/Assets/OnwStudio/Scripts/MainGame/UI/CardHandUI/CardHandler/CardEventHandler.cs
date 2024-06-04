using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class CardEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Action _enterAction = null;
    private Action _exitAction = null;
    private Action _clickAction = null;

    public void AddListenerPointerEnterAction(Action action)
        => _enterAction += action;

    public void AddListenerPointerExitAction(Action action)
        => _exitAction += action;

    public void AddListenerPointerClickAction(Action action)
        => _clickAction += action;

    public void RemoveListenerPointerEnterAction(Action action)
        => _enterAction -= action;

    public void RemoveListenerPointerExitAction(Action action)
        => _exitAction -= action;

    public void RemoveListenerPointerClickAction(Action action)
        => _clickAction -= action;

    public void RemoveAllListenerPointerEnterAction()
        => _enterAction = null;

    public void RemoveAllListenerPointerExitAction()
        => _exitAction = null;

    public void RemoveAllListenerPointerClickAction()
        => _clickAction = null;

    public void OnPointerClick(PointerEventData eventData)
        => _clickAction?.Invoke();

    public void OnPointerEnter(PointerEventData eventData)
        => _enterAction?.Invoke();

    public void OnPointerExit(PointerEventData eventData)
        => _exitAction?.Invoke();
}
