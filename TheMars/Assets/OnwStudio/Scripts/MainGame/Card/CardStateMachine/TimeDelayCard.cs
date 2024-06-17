using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDelayCard : CardStateMachine
{
    public float DelayTime { get; internal set; } = 1f;

    public override void OnUseEnded<T>(T cardController)
    {
        cardController.OnDelaySeconds.Invoke(cardController, DelayTime);
    }
}
