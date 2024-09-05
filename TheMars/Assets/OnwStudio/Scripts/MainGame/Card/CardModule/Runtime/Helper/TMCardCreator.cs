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
        [SerializeField] private TMCardModel _templatePrefab;

        public List<TMCardModel> CreateCards(int createCount, bool shouldInitialize = true)
        {
            if (_cards.Count <= 0) return null;

            List<TMCardModel> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                cardList.Add(CreateCardByCardData(_cards[Random.Range(0, _cards.Count)], shouldInitialize));
            }

            return cardList;
        }

        public TMCardModel CreateCardByCardData(TMCardData cardData, bool shouldInitialize = true)
        {
            TMCardModel card = Object.Instantiate(_templatePrefab);
            card.CardData = cardData;

            if (shouldInitialize)
            {
                card.Initialize();
            }

            return card;
        }

        public TMCardModel CreateCard(bool shouldInitialize = true)
        {
            return _cards.Count > 0 ? CreateCardByCardData(_cards[Random.Range(0, _cards.Count)], shouldInitialize) : null;
        }
    }
}