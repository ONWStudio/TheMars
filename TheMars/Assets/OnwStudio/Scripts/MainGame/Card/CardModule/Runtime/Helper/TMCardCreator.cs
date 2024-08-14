using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.ScriptableObjects;
using Onw.ServiceLocator;
using Onw.Attribute;
using Onw.Manager;

namespace TMCard.Runtime
{
    [System.Serializable]
    public sealed class TMCardCreator
    {
        [SerializeField] private List<TMCardData> _cards = new();
        [SerializeField] private TMCardController _templatePrefab = null;

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