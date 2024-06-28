using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;

namespace TMCard.UI
{
    public sealed class TMCardUICreator : Singleton<TMCardUICreator>
    {
        [SerializeField] private List<TMCardData> _cards = new();

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
                cardList.Add(new GameObject(cardData.name).AddComponent<TMCardController>());
                cardList[i].CardData = cardData;
            }

            return cardList;
        }

        public TMCardController CreateCard()
        {
            if (_cards.Count <= 0) return null;

            TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
            TMCardController cardUI = new GameObject(cardData.name).AddComponent<TMCardController>();
            cardUI.CardData = cardData;

            return cardUI;
        }
    }
}