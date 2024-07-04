using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;

namespace TMCard.UI
{
    public sealed class TMCardUICreator : Singleton<TMCardUICreator>
    {
        [SerializeField] private List<TMCardData> _cards = new();

        [SerializeField] private TMCardController _templatePrefab = null;

        protected override void Init()
        {
        }

        public List<TMCardController> CreateCards(int createCount)
        {
            if (_cards.Count <= 0) return null;

            List<TMCardController> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
                cardList.Add(Instantiate(_templatePrefab));
                cardList[i].CardData = cardData;
                cardList[i].Initialize();
            }

            return cardList;
        }

        public TMCardController CreateCard()
        {
            if (_cards.Count <= 0) return null;

            TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
            TMCardController cardUI = Instantiate(_templatePrefab);
            cardUI.CardData = cardData;
            cardUI.Initialize();

            return cardUI;
        }
    }
}