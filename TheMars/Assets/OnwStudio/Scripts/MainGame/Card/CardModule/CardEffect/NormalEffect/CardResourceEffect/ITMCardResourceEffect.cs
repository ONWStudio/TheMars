using System.Collections.Generic;
using Onw.Interface;
using TM.Class;

namespace TM.Card.Effect
{
    public interface ITMCardResourceEffect : ITMNormalEffect
    {
        IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources { get; }

        void AddResource(TMResourceKind resourceKind, int additionalAmount);
    }
}