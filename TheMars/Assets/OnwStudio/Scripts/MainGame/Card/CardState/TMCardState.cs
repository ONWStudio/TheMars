using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TMCardState
{
    public UnityEvent OnCustomEffect { get; private set; } = new();

    protected abstract void OnEffect<T>(T cardController) where T : TMCardController<T>;

    public void OnFire<T>(T cardController) where T : TMCardController<T>
    {
        OnEffect(cardController);
        OnCustomEffect.Invoke();
    }
}