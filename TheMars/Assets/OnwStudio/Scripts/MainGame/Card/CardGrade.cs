using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class Card : ScriptableObject
{
    /// <summary>
    /// .. 발동 효과에 따른 카드 등급
    /// </summary>
    public enum CARD_GRADE : byte
    {
        /// <summary> .. 일반 </summary>>
        NORMAL,
        /// <summary> .. 고급 </summary>>
        HIGH,
        /// <summary> .. 희귀 </summary>>
        RARE,
        /// <summary> .. 유일 </summary>>
        ONLY
    }
}
