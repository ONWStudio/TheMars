using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMCardUISystemModules
{
    [DisallowMultipleComponent]
    public sealed class TMCardUICreator : SceneSingleton<TMCardUICreator>
    {
        public override string SceneName => "MainGameScene";
        protected override void Init() { }

        public List<TMCardUIController> CreateCards(int createCount)
        {
            List<TMCardUIController> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                cardList.Add(new GameObject($"Card_{i + 1}").AddComponent<TMCardUIController>());
            }

            return cardList;
        }

        public TMCardUIController CreateCard()
            => new GameObject("Card").AddComponent<TMCardUIController>();
    }
}