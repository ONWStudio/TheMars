using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TM.Card.Runtime
{
    [Serializable]
    public sealed class TMCardCreator
    {
        public event UnityAction<TMCardModel> OnPostCreateCard
        {
            add => _onPostCreateCard.AddListener(value);
            remove => _onPostCreateCard.RemoveListener(value);
        }
        
        public event UnityAction<TMCardModel> OnPreCreateCard
        {
            add => _onPreCreateCard.AddListener(value);
            remove => _onPreCreateCard.RemoveListener(value);
        }

        [SerializeField] private List<TMCardData> _cards = new();
        [SerializeField] private TMCardModel _templatePrefab;

        [Header("Event")]
        [SerializeField] private UnityEvent<TMCardModel> _onPreCreateCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onPostCreateCard = new();
        
        public TMCardModel[] CreateRandomCards(int creationCount, bool shouldInitialize = true)
        {
            if (_cards.Count <= 0) return null;
            
            TMCardModel[] cardArray = new TMCardModel[creationCount];

            for (int i = 0; i < creationCount; i++)
            {
                cardArray[i] = createCardByCardData(_cards[Random.Range(0, _cards.Count)], shouldInitialize);
            }

            return cardArray;
        }

        public TMCardModel[] CreateRandomCardsByWhere(Func<TMCardData, bool> predicate, int creationCount, bool shouldInitialize = true)
        {
            if (_cards.Count <= 0) return null;

            TMCardData[] filterCards = _cards
                .Where(predicate)
                .ToArray();
            
            if (filterCards.Length > 0)
            {
                TMCardModel[] cardArray = new TMCardModel[creationCount];
            
                for (int i = 0; i < creationCount; i++)
                {
                    cardArray[i] = createCardByCardData(filterCards[Random.Range(0, filterCards.Length)]);
                }

                return cardArray;
            }

            return Array.Empty<TMCardModel>();
        }

        public TMCardModel CreateRandomCard(bool shouldInitialize = true)
        {
            return _cards.Count > 0 ? createCardByCardData(_cards[Random.Range(0, _cards.Count)], shouldInitialize) : null;
        }

        public TMCardModel CreateRandomCardByWhere(Func<TMCardData, bool> predicate, bool shouldInitialize = true)
        {
            TMCardData[] filterCards = _cards.Where(predicate).ToArray();
            TMCardModel currentCard = null;
            
            if (filterCards.Length > 0)
            {
                TMCardData current = filterCards[Random.Range(0, filterCards.Length)];
                currentCard = createCardByCardData(current, shouldInitialize);
            }

            return currentCard;
        }
        
        private TMCardModel createCardByCardData(TMCardData cardData, bool shouldInitialize = true)
        {
            TMCardModel card = Object.Instantiate(_templatePrefab);
            _onPreCreateCard.Invoke(card);
            
            card.CardData = cardData;

            if (shouldInitialize)
            {
                card.Initialize();
            }
            
            _onPostCreateCard.Invoke(card);

            return card;
        }
    }
}