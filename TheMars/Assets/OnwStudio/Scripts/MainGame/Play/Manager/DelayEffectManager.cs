using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMCardUISystemModules;
using CoroutineExtensions;

/// <summary>
/// .. 해당 클래스는 Effect를 받아서 N턴 또는 N초의 시간이 흐른 후 스킬이나 카드효과를 발동시키는 클래스입니다
/// </summary>
public sealed class DelayEffectManager : SceneSingleton<DelayEffectManager>
{
    public override string SceneName => "MainGameScene";
    public bool _isWaitTurn = false;

    protected override void Init() {}

    public void OnNextTurn()
    {
        _isWaitTurn = true;
    }

    /// <summary>
    /// .. 특정 시간 (초 단위) 후 카드 효과를 발동시킵니다
    /// </summary>
    /// <param name="delayTime"> .. 딜레이 시킬 시간 </param>
    /// <param name="onNotifyRemainingTime"> .. 콜백 메서드로 남은 시간을 받아옵니다 1초마다 콜백을 호출시킵니다 </param>
    public void WaitForSecondsEffect(TMCardUIController owner, float delayTime, Action<float> onNotifyRemainingTime)
        => StartCoroutine(iEWaitForSecondsEffect(owner, delayTime, onNotifyRemainingTime));

    public void WaitForTurnCountEffect(TMCardUIController owner, int delayTurn, Action<int> onNotifyTurnCount)
        => StartCoroutine(iEWaitForTurnCountEffect(owner, delayTurn, onNotifyTurnCount));

    private IEnumerator iEWaitForSecondsEffect(TMCardUIController owner, float delayTime, Action<float> onNotifyRemainingTime)
    {
        float prevTime = delayTime;
        while (delayTime > 0f)
        {
            yield return null;
            delayTime -= Time.deltaTime;

            if (prevTime - delayTime >= 1.0f)
            {
                onNotifyRemainingTime?.Invoke(delayTime);
                prevTime = delayTime;
            }
        }

        onNotifyRemainingTime?.Invoke(-1f);
        owner.CardData.UseCard(owner.gameObject);
    }

    private IEnumerator iEWaitForTurnCountEffect(TMCardUIController owner, int delayTurn, Action<int> onNotifyTurnCount)
    {
        while (delayTurn > 0)
        {
            yield return new WaitUntil(() => _isWaitTurn);

            _isWaitTurn = false;
            delayTurn--;
            onNotifyTurnCount?.Invoke(delayTurn);
        }

        onNotifyTurnCount?.Invoke(0);
        owner.CardData.UseCard(owner.gameObject);
    }
}
