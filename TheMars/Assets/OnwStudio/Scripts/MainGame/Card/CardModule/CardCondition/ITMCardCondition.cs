using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.AddtionalCondition
{
    public interface ITMCardAddtionalCondition
    {
        bool AdditionalCondition { get; }
    }

    public interface ITMCardAddtionalCondtionProcessing
    {
        void UseProcessing();
    }
}