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
                cardList.Add(CreateCardByCardData(_cards[Random.Range(0, _cards.Count)]));
                
                if (ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service))
                {
                    service.AddListenerToCard(cardList[i]);
                }
            }

            return cardList;
        }

        public TMCardController CreateCardByCardData(TMCardData cardData)
        {
            TMCardController card = Object.Instantiate(_templatePrefab);
            card.CardData = cardData;
            card.Initialize();

            return card;
        }

        public TMCardController CreateCard()
        {
            return _cards.Count > 0 ? CreateCardByCardData(_cards[Random.Range(0, _cards.Count)]) : null;
        }
    }
}