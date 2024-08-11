using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.Effect
{
    public interface ITMInnerEffector
    {
        public IReadOnlyList<ITMNormalEffect> InnerEffect { get; }
    }
}