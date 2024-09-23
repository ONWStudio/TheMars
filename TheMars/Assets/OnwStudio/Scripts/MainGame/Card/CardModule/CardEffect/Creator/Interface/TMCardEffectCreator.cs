namespace TM.Card.Effect.Creator
{
    public abstract class TMCardEffectCreator
    {
        protected static TEffect CreateEffect<TEffect, TCreator>(TCreator creator)
            where TEffect : ITMCardEffect, new()
            where TCreator : TMCardEffectCreator
        {
            TEffect effect = new();

            if (effect is ITMCardInitializeEffect<TCreator> initializeEffect)
            {
                initializeEffect.Initialize(creator);
            }

            return effect;
        }

        public abstract ITMCardEffect CreateEffect();
    }
}