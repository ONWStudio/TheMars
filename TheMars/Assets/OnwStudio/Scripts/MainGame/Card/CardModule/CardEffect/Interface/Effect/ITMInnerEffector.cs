using System.Collections.Generic;
namespace TMCard.Effect
{
    public interface ITMInnerEffector
    {
        public IReadOnlyList<ITMNormalEffect> InnerEffect { get; }
    }
}