using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;
using UnityEngine.Serialization;

namespace TMCard.Runtime
{
    public sealed class TMDelayEffectManager : MonoBehaviour
    {
        [SerializeField]
        private bool _isWaitTurn = false;

        public void OnNextTurn()
        {
            _isWaitTurn = true;
        }

        /// <summary>
        /// .. 특정 시간 (초 단위) 후 카드 효과를 발동시킵니다
        /// </summary>
        /// <param name="delayTime"> .. 딜레이 시킬 시간 </param>
        /// <param name="onSuccessSeconds"></param>
        /// <param name="onNotifyRemainingTime"> .. 콜백 메서드로 남은 시간을 받아옵니다 1초마다 콜백을 호출시킵니다 </param>
        public void WaitForSecondsEffect(float delayTime, Action onSuccessSeconds, Action<float> onNotifyRemainingTime)
            => StartCoroutine(iEWaitForSecondsEffect(delayTime, onSuccessSeconds, onNotifyRemainingTime));

        /// <summary>
        /// .. 특정 카운트만큼 턴 진행 후 카드 효과를 발동시킵니다
        /// </summary>
        /// <param name="delayTurn"> .. 얼마나 턴을 기다릴 것인가 </param>
        /// <param name="onSuccessTurn"></param>
        /// <param name="onNotifyTurnCount"> .. 매턴마다 호출되는 콜백 메서드 입니다 </param>
        public void WaitForTurnCountEffect(int delayTurn, Action onSuccessTurn, Action<int> onNotifyTurnCount)
            => StartCoroutine(iEWaitForTurnCountEffect(delayTurn, onSuccessTurn, onNotifyTurnCount));

        private static IEnumerator iEWaitForSecondsEffect(float delayTime, Action onSuccessSeconds, Action<float> onNotifyRemainingTime)
        {
            var prevTime = delayTime;
            while (delayTime > 0f)
            {
                yield return null;
                delayTime -= Time.deltaTime;

                if (!(prevTime - delayTime >= 1.0f)) continue;
                onNotifyRemainingTime?.Invoke(delayTime);
                prevTime = delayTime;
            }

            onNotifyRemainingTime?.Invoke(-1f);
            onSuccessSeconds?.Invoke();
        }

        private IEnumerator iEWaitForTurnCountEffect(int delayTurn, Action onSuccessTurn, Action<int> onNotifyTurnCount)
        {
            while (delayTurn > 0)
            {
                yield return new WaitUntil(() => _isWaitTurn);

                _isWaitTurn = false;
                delayTurn--;
                onNotifyTurnCount?.Invoke(delayTurn);
            }

            onNotifyTurnCount?.Invoke(0);
            onSuccessTurn?.Invoke();
        }
    }
}
