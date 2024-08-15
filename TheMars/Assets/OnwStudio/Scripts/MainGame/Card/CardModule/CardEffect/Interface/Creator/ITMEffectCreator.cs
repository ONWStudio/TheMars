namespace TMCard.Effect
{
    public interface ITMEffectCreator
    {
        protected static class EffectGenerator
        {
            public static TEffect CreateEffect<TEffect, TCreator>(TCreator creator) where TEffect : ITMCardEffect, new() where TCreator : ITMEffectCreator
            {
                TEffect effect = new();

                if (effect is ITMInitializableEffect<TCreator> initializeableEffect)
                {
                    initializeableEffect.Initialize(creator);
                }

                return effect;
            }
        }

        ITMCardEffect CreateEffect();
    }
}