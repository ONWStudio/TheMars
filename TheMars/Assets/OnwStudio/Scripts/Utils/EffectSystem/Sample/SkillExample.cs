using System.Collections;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

namespace Onw.EffectSystem.Sample
{
    public class SkillExample : MonoBehaviour, IRootEffectTrigger
    {
        private class SkillEffect : BaseEffect<SkillExample>
        {
            public override void ApplyEffectOverride(SkillExample rootEffectTrigger)
            {
                rootEffectTrigger.Test(() =>
                {
                    OnEffectEnded?.Invoke(this);
                });
            }
        }

        public IReadOnlyDictionary<string, SerializedBaseEffect> RootTriggers => _rootTriggers;
        [SerializeField, SerializedDictionary] private SerializedDictionary<string, SerializedBaseEffect> _rootTriggers;

        private void Start()
        {
            if (_rootTriggers.TryGetValue("Start", out SerializedBaseEffect serialize))
            {
                serialize.Effect.ApplyEffect(this);
            }
        }

        public void Test(Action action)
        {
            StartCoroutine(iETest(action));
        }

        private IEnumerator iETest(Action action)
        {
            yield return new WaitForSeconds(2.0f);
            action.Invoke();
        }
    }
}