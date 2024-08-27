using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using TM.Building.Effect.Creator;

namespace TM.Building.Effect
{
    public sealed class GetMarsLithiumEffect : ITMBuildingEffect, ITMBuildingInitializeEffect<GetMarsLithiumEffectCreator>
    {
        [SerializeField, ReadOnly] private int _levelOneRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelOneMarsLithium = 1;

        [SerializeField, ReadOnly] private int _levelTwoRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelTwoMarsLithium = 1;

        [SerializeField, ReadOnly] private int _levelThreeRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelThreeMarsLithium = 1;

        private Coroutine _coroutine = null;
        
        public void Initialize(GetMarsLithiumEffectCreator effectCreator)
        {
            _levelOneRepeatSeconds = effectCreator.LevelOneRepeatSeconds;
            _levelOneMarsLithium = effectCreator.LevelOneMarsLithium;
            _levelTwoRepeatSeconds = effectCreator.LevelTwoRepeatSeconds;
            _levelTwoMarsLithium = effectCreator.LevelTwoMarsLithium;            
            _levelThreeRepeatSeconds = effectCreator.LevelThreeRepeatSeconds;
            _levelThreeMarsLithium = effectCreator.LevelThreeMarsLithium;
        }
        
        public void ApplyEffectLevelOne(TMBuilding owner)
        {
            fireEffect(owner, _levelOneRepeatSeconds, _levelOneMarsLithium);
        }
        
        public void ApplyEffectLevelTwo(TMBuilding owner)
        {
            fireEffect(owner, _levelTwoRepeatSeconds, _levelTwoMarsLithium);
        }
        
        public void ApplyEffectLevelThree(TMBuilding owner)
        {
            fireEffect(owner, _levelThreeRepeatSeconds, _levelThreeMarsLithium);
        }
        
        public void DisableEffect(TMBuilding owner)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
        }

        private void fireEffect(TMBuilding owner, int repeatSeconds, int marsLithium)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
            _coroutine = owner.StartCoroutine(iEFireEffect(repeatSeconds, marsLithium));
        }

        private static IEnumerator iEFireEffect(int repeatSeconds, int marsLithium)
        {
            while (true)
            {
                yield return CoroutineHelper.WaitForSeconds(repeatSeconds);
                Debug.Log("마르스 리튬 획득!");
                PlayerManager.Instance.MarsLithium += marsLithium;
            }
        }
    }
}
