using System.Collections;
using System.Collections.Generic;
using TcgEngine;
using UnityEngine;

namespace TMCardUISystemModules
{
    public sealed class TMCardUICreator : Singleton<TMCardUICreator>
    {
        [SerializeField] private List<TMCardData> _cards = new();

        protected override void Init()
        {
        }

        public List<TMCardUIController> CreateCards(int createCount)
        {
            if (_cards.Count <= 0) return null;

            List<TMCardUIController> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
                cardList.Add(new GameObject(cardData.name).AddComponent<TMCardUIController>());
                cardList[i].CardData = cardData;
            }

            return cardList;
        }

        public TMCardUIController CreateCard()
        {
            if (_cards.Count <= 0) return null;

            TMCardData cardData = _cards[Random.Range(0, _cards.Count)];
            TMCardUIController cardUI = new GameObject(cardData.name).AddComponent<TMCardUIController>();
            cardUI.CardData = cardData;

            return cardUI;
        }
    }
}