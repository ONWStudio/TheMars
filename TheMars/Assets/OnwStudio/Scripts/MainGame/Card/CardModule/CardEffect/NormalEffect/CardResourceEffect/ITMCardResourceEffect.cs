using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMCard.Runtime;

namespace TMCard.Effect.Resource
{
    public interface ITMCardResourceEffect : ITMNormalEffect, IAmountHolder
    {
        /// <summary>
        /// .. 외부에서 TMCardResourceEffect에 직접 접근하여 쓰는 경우가 있을때 사용하는 메서드 입니다
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="addtionalAmount"></param>
        void AddRequiredResource(int addtionalAmount);
    }
}