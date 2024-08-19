namespace TMCard.Effect
{
    public interface ITMInitializeEffect<in T> where T : ITMEffectCreator
    {
        void Initialize(T effectCreator);
    }
}