using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Buff.Trigger
{
    public interface ITMBuffTrigger
    {
        protected static TBuff CreateBuff<TBuff, TTrigger>(TTrigger creator)
            where TBuff : TMBuffBase, new()
            where TTrigger : ITMBuffTrigger
        {
            TBuff effect = new();

            if (effect is ITMInitializeBuff<TTrigger> initializeEffect)
            {
                initializeEffect.Initialize(creator);
            }

            return effect;
        }

        TMBuffBase CreateBuff();
    }

    [System.Serializable]
    public abstract class TMRepeatBuffTrigger : ITMBuffTrigger
    {
        [field: SerializeField] public int RepeatTime { get; private set; }
        
        public abstract TMBuffBase CreateBuff();
    }

    public interface ITMInitializeBuff<in TCreator> where TCreator : ITMBuffTrigger
    {
        void Initialize(TCreator creator);
    }

    public class TMSatisfactionAddBuffTrigger : ITMBuffTrigger
    {
        public TMBuffBase CreateBuff()
        {
            return ITMBuffTrigger.CreateBuff<TMSatisfactionAddBuff, TMSatisfactionAddBuffTrigger>(this);
        }
    }
}