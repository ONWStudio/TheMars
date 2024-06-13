using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubClassSelectorSpace;

namespace TMCardUISystemModules
{
    [DisallowMultipleComponent, RequireComponent(typeof(EventReceiver))]
    public class TMCardGameUIManager : SceneSingleton<TMCardGameUIManager>
    {
        public override string SceneName => "MainGameScene";

        [field: SerializeReference, SubClassSelector(typeof(ICardSorter))] public ICardSorter CardSorter { get; private set; } = null;

        public EventReceiver EventReceiver { get; private set; }

        public RectTransform DeckTransform { get; private set; }

        protected override void Init()
        {
            EventReceiver = GetComponent<EventReceiver>();
        }
    }
}