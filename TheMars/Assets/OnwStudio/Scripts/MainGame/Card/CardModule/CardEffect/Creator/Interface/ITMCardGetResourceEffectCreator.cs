using System.Collections.Generic;
using TM.Class;

namespace TM.Card.Effect.Creator
{
    public interface ITMCardGetResourceEffectCreator : ITMCardNormalEffectCreator
    {
        IReadOnlyList<TMResourceData> Resources { get; }   
    }
}
