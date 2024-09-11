using System.Collections.Generic;
using Onw.Interface;
using TM.Class;

namespace TM.Card.Effect
{
    public interface ITMCardResourceEffect : ITMNormalEffect, INotifier
    {
        IReadOnlyDictionary<TMResourceKind, TMResourceDataForRuntime> Resources { get; }

        void AddResource(TMResourceKind resourceKind, int additionalAmount);
    }
}