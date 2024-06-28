using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard
{
    public interface ICardCondition
    {
        bool AdditionalCondition { get; }
    }

    public interface ICardCondtionProcessing
    {
        void UseProcessing();
    }
}