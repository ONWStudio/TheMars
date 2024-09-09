using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.ServiceLocator;
using TM.Building.Effect.Creator;
using TM.Manager;

namespace TM.Building.Effect
{
    public abstract class TMBuildingResourceEffect : ITMBuildingResourceEffect, ITMBuildingInitializeEffect<GetResourceEffectCreator>
    {
        [SerializeField, ReadOnly] private float _levelOneRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelOneResource = 1;

        [SerializeField, ReadOnly] private float _levelTwoRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelTwoResource = 1;

        [SerializeField, ReadOnly] private float _levelThreeRepeatSeconds = 0;
        [SerializeField, ReadOnly] private int _levelThreeResource = 1;
        
        private Coroutine _coroutine = null;
        
        public void Initialize(GetResourceEffectCreator effectCreator)
        {
            _levelOneRepeatSeconds = effectCreator.LevelOneRepeatSeconds;
            _levelOneResource = effectCreator.LevelOneResource;
            _levelTwoRepeatSeconds = effectCreator.LevelTwoRepeatSeconds;
            _levelTwoResource = effectCreator.LevelTwoResource;
            _levelThreeRepeatSeconds = effectCreator.LevelThreeRepeatSeconds;
            _levelThreeResource = effectCreator.LevelThreeResource;
        }
        
        public void ApplyEffectLevelOne(TMBuilding owner)
        {
            fireEffect(owner, _levelOneRepeatSeconds, _levelOneResource);
        }

        public void ApplyEffectLevelTwo(TMBuilding owner)
        {
            fireEffect(owner, _levelTwoRepeatSeconds, _levelTwoResource);
        }

        public void ApplyEffectLevelThree(TMBuilding owner)
        {
            fireEffect(owner, _levelThreeRepeatSeconds, _levelThreeResource);
        }

        public void DisableEffect(TMBuilding owner)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
        }

        private void fireEffect(TMBuilding owner, float repeatSeconds, int marsLithium)
        {
            owner.StopCoroutineIfNotNull(_coroutine);
            _coroutine = owner.StartCoroutine(iEFireEffect(repeatSeconds, marsLithium));
        }

        private static IEnumerator iEFireEffect(float repeatSeconds, int marsLithium)
        {
            float timeAccumulator = 0f;

            while (true)
            {
                timeAccumulator += Time.deltaTime * TimeManager.GameSpeed;

                if (timeAccumulator >= repeatSeconds)
                {
                    Debug.Log("마르스 리튬 획득!");
                    ServiceLocator<PlayerManager>.InvokeService(player => player.MarsLithium += marsLithium);
                    timeAccumulator -= repeatSeconds;
                }

                yield return null;
            }
        }
    }
    public interface ITMBuildingResourceEffect : ITMBuildingEffect {}

    public interface ITMBuildingEffect
    {
        void ApplyEffectLevelOne(TMBuilding owner);
        void ApplyEffectLevelTwo(TMBuilding owner);
        void ApplyEffectLevelThree(TMBuilding owner);
        void DisableEffect(TMBuilding owner);
    }
}

