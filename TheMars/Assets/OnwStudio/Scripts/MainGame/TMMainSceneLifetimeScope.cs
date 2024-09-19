using System;
using TM.Card.Effect;
using UnityEngine;
using TM.Card.Runtime;
using TM.Card.UI;
using TM.Grid;
using VContainer;
using VContainer.Unity;
using TM.Manager;
using TM.Synergy;

namespace TM
{
    public class TMMainSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private TMCardCollectUIController _collectUIController;
        [SerializeField] private TMCardManager _cardManager;
        [SerializeField] private TMGridManager _gridManager;
        [SerializeField] private TMSynergyManager _synergyManager;
        [SerializeField] private TMSimulator _simulator;
        [SerializeField] private PlayerManager _player;
        [SerializeField] private TMCardNotifyIconSpawner _iconSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_collectUIController);
            builder.RegisterComponent(_synergyManager);
            builder.RegisterComponent(_cardManager);
            builder.RegisterComponent(_gridManager);
            builder.RegisterComponent(_simulator);
            builder.RegisterComponent(_player);
            builder.RegisterBuildCallback(onPostBuild);
        }

        private void onPostBuild(IObjectResolver container)
        {
            _cardManager.CardCreator.OnPreCreateCard += Container.Inject;
            _cardManager.CardCreator.OnPostCreateCard += cardModel =>
            {
                if (cardModel.CardEffect is null) return;
                
                Container.Inject(cardModel.CardEffect);
                if (cardModel.CardEffect is IPostStartable stable) // .. 별도의 커스텀 인터페이스 제공
                {
                    stable.PostStart();
                }
            };
            _iconSpawner.OnCreateIcon += collectIcon =>
            {
                Container.Inject(collectIcon);
                if (collectIcon is IPostStartable stable)
                {
                    stable.PostStart();
                }
            };
        }
    }
}
