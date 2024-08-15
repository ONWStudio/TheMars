using System;
using System.Collections.Generic;
using Onw.ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
namespace TMCard.Runtime
{
    [Serializable]
    public sealed class TMCardCreator
    {
        [SerializeField] private List<TMCardData> _cards = new();
        [SerializeField] private TMCardController _templatePrefab;

        public List<TMCardController> CreateCards(int createCount)
        {
            if (_cards.Count <= 0) return null;

            List<TMCardController> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                var cardData = _cards[Random.Range(0, _cards.Count)];
                cardList.Add(Object.Instantiate(_templatePrefab));
                cardList[i].CardData = cardData;
                cardList[i].Initialize();
                
                if (ServiceLocator<ITMCardService>.TryGetService(out var service))
                {
                    service.AddListenerToCard(cardList[i]);
                }
            }

            return cardList;
        }

        public TMCardController CreateCard()
        {
            if (_cards.Count <= 0) return null;

            var cardData = _cards[Random.Range(0, _cards.Count)];
            var card = Object.Instantiate(_templatePrefab);
            card.CardData = cardData;
            card.Initialize();

            return card;
        }
    }
}