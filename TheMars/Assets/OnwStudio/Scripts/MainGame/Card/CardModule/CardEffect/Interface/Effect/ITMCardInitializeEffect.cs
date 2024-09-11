
using TM.Card.Effect.Creator;

namespace TM.Card.Effect
{
    public interface ITMCardInitializeEffect<in T> where T : ITMCardEffectCreator
    {
        void Initialize(T effectCreator);
    }
}