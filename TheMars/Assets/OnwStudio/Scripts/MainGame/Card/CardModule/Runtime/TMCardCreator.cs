using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.ScriptableObjects;
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
                TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
                cardList.Add(Object.Instantiate(_templatePrefab));
                cardList[i].CardData = cardData;
                cardList[i].Initialize();
            }

            return cardList;
        }

        public TMCardController CreateCard()
        {
            if (_cards.Count <= 0) return null;

            TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
            TMCardController card = Object.Instantiate(_templatePrefab);
            card.CardData = cardData;
            card.Initialize();

            return card;
        }
    }
}