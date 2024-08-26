namespace TMCard.Effect
{
    public interface ITMCardEffectCreator
    {
        protected static class CardEffectGenerator
        {
            public static TEffect CreateEffect<TEffect, TCreator>(TCreator creator) where TEffect : ITMCardEffect, new() where TCreator : ITMCardEffectCreator
            {
                TEffect effect = new();

                if (effect is ITMCardInitializeEffect<TCreator> initializeEffect)
                {
                    initializeEffect.Initialize(creator);
                }

                return effect;
            }
        }

        ITMCardEffect CreateEffect();
    }
}