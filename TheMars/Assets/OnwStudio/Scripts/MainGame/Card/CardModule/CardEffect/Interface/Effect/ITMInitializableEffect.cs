namespace TMCard.Effect
{
    public interface ITMInitializableEffect<T> where T : ITMEffectCreator
    {
        void Initialize(T effectCreator);
    }
}