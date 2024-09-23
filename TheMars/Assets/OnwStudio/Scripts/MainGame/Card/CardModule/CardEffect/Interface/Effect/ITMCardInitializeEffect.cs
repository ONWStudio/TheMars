
using TM.Card.Effect.Creator;

namespace TM.Card.Effect
{
    public interface ITMCardInitializeEffect<in T> where T : TMCardEffectCreator
    {
        void Initialize(T effectCreator);
    }
}