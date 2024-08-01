using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Onw.EffectSystem
{
    public interface IRootEffectTrigger
    {
        IReadOnlyDictionary<string, SerializedBaseEffect> RootTriggers { get; }
    }

    [Serializable]
    public sealed class SerializedBaseEffect
    {
        [field: SerializeField] public BaseEffect Effect { get; set; }
    }

    [Serializable]
    public abstract class BaseEffect
    {
        public Action<BaseEffect> OnEffectEnded { get; internal set; }
        public abstract void ApplyEffect(IRootEffectTrigger rootEffectTrigger);
    }

    public abstract class BaseEffect<T> : BaseEffect where T : IRootEffectTrigger
    {
        public abstract void ApplyEffectOverride(T rootEffectTrigger);

        public override sealed void ApplyEffect(IRootEffectTrigger rootEffectTrigger)
        {
            if (rootEffectTrigger is not T rootOverride)
            {
                Debug.LogWarning("해당 타입의 스킬이 될 수 없습니다");
                OnEffectEnded?.Invoke(this);
                return;
            }

            ApplyEffectOverride(rootOverride);
        }
    }

    public sealed class ParallelEffect : BaseEffect
    {
        [SerializeReference, SerializeReferenceDropdown] private List<BaseEffect> _effects = new();

        public override void ApplyEffect(IRootEffectTrigger rootEffectTrigger)
        {
            foreach (var effect in _effects)
            {
                effect.OnEffectEnded += OnChildEffectEnded;
                effect.ApplyEffect(rootEffectTrigger);
            }
        }

        private void OnChildEffectEnded(BaseEffect effect)
        {
            effect.OnEffectEnded -= OnChildEffectEnded;

            if (_effects.All(e => e == effect || (e.OnEffectEnded?.GetInvocationList().Length ?? 0) <= 0))
            {
                OnEffectEnded?.Invoke(this);
            }
        }

        public void AddEffect(BaseEffect effect)
        {
            _effects.Add(effect);
        }
    }

    public class SequenceEffect : BaseEffect
    {
        [SerializeField, SerializeReferenceDropdown] private List<BaseEffect> _effects = new();
        private int _currentIndex = 0;

        public override void ApplyEffect(IRootEffectTrigger rootEffectTrigger)
        {
            _currentIndex = 0;
            ApplyNextEffect(rootEffectTrigger);
        }

        private void ApplyNextEffect(IRootEffectTrigger rootEffectTrigger)
        {
            if (_currentIndex < _effects.Count)
            {
                var effect = _effects[_currentIndex];
                effect.OnEffectEnded += onChildEffectEnded;
                effect.ApplyEffect(rootEffectTrigger);
            }
            else
            {
                OnEffectEnded?.Invoke(this);
                Debug.Log("Sequence Ended");
            }

            void onChildEffectEnded(BaseEffect effect)
            {
                effect.OnEffectEnded -= onChildEffectEnded;
                _currentIndex++;
                ApplyNextEffect(rootEffectTrigger);
            }
        }

        public void AddEffect(BaseEffect effect)
        {
            _effects.Add(effect);
        }
    }
}