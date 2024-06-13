using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_SPECIAL_EFFECT
{
    /// <summary>
    /// .. 특수능력 없음
    /// </summary>
    NONE = 0,
    /// <summary>
    /// .. 일회용
    /// </summary>    
    DISPOSABLE,
    /// <summary>
    /// .. 재활용
    /// </summary>
    RECYCLING,
    /// <summary>
    /// .. 신기루
    /// </summary>
    MIRAGE,
    /// <summary>
    /// .. 버리기
    /// </summary>
    DROP,
    /// <summary>
    /// .. 소요(n턴 또는 n 시간 후 효과 발동)
    /// </summary>
    DELAY,
    /// <summary>
    /// .. 지속 (n턴 또는 n 시간 동안 효과 지속)
    /// </summary>
    CONTINUING,
    /// <summary>
    /// .. 보유
    /// </summary>
    HOLD,
    /// <summary>
    /// .. 반복
    /// </summary>
    REPEAT,
    /// <summary>
    /// .. 드로우
    /// </summary>
    DRAW
}

