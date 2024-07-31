using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.EffectSystem
{
    public class EffectTrigger
    {
        public void FireEffect(IBaseEffect effect)
        {

        }
    }

    public interface IRootEffectTrigger
    {
        IReadOnlyDictionary<string, EffectTrigger> RootTriggers { get; }
    }

    public interface IBaseEffect
    {
        void ApplyEffect(IRootEffectTrigger rootEffectTrigger, EffectTrigger trigger);
        void OnEndedEffect(IRootEffectTrigger rootEffectTrigger, EffectTrigger trigger);
    }

    public interface IEffectTrigger
    {
        EffectTrigger EffectTrigger { get; }
    }

    // .. 여러 이펙트의 동시 실행
    public class ParalleEffect : IBaseEffect, IEffectTrigger
    {
        private List<IBaseEffect> _baseEffect = new();

        public EffectTrigger EffectTrigger { get; } = new();

        public void ApplyEffect(IRootEffectTrigger rootEffectTrigger, EffectTrigger trigger)
        {
            _baseEffect.ForEach(rootEffectTrigger.)
        }

        public void OnEndedEffect(IRootEffectTrigger rootEffectTrigger, EffectTrigger trigger)
        {
        }
    }
}
