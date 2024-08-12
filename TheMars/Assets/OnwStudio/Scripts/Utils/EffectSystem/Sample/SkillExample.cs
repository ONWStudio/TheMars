using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Onw.State;

namespace Onw.EffectSystem.Sample
{

    public class SkillExample : MonoBehaviour
    {
        private void Start()
        {
            //if (_rootTriggers.TryGetValue("Start", out Effect serialize))
            //{
            //    serialize.ApplyEffect(this);
            //}
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