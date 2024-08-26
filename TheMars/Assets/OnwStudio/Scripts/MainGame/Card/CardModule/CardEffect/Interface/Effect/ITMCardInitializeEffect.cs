namespace TMCard.Effect
{
    public interface ITMCardInitializeEffect<in T> where T : ITMCardEffectCreator
    {
        void Initialize(T effectCreator);
    }
}