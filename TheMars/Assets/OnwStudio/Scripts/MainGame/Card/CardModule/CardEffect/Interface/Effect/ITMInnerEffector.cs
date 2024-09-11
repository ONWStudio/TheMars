using System.Collections.Generic;

namespace TM.Card.Effect
{
    public interface ITMInnerEffector
    {
        public IReadOnlyList<ITMNormalEffect> InnerEffect { get; }
    }
}