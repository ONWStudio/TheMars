using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Serialization;
using TMPro;
using Onw.Prototype;
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

        [FormerlySerializedAs("_templatePrefab")]
        [SerializeField] private AssetReferenceGameObject _templateReference;

        [Header("Event")]
        [SerializeField] private UnityEvent<TMCardModel> _onPreCreateCard = new();
        [SerializeField] private UnityEvent<TMCardModel> _onPostCreateCard = new();

        public TMCardModel[] CreateRandomCards(int creationCount, bool shouldInitialize = true)
        {
            if (TMCardDataManager.Instance.CardDataList.Count <= 0) return null;

            TMCardModel[] cardArray = new TMCardModel[creationCount];

            for (int i = 0; i < creationCount; i++)
            {
                cardArray[i] = createCardByCardData(
                    TMCardDataManager.Instance.CardDataList[Random.Range(0, TMCardDataManager.Instance.CardDataList.Count)],
                    shouldInitialize);
            }

            return cardArray;
        }

        public TMCardModel[] CreateRandomCardsByWhere(Func<TMCardData, bool> predicate, int creationCount, bool shouldInitialize = true)
        {
            if (TMCardDataManager.Instance.CardDataList.Count <= 0) return null;

            TMCardData[] filterCards = TMCardDataManager.Instance.CardDataList
                .Where(predicate)
                .ToArray();

            if (filterCards.Length > 0)
            {
                TMCardModel[] cardArray = new TMCardModel[creationCount];

                for (int i = 0; i < creationCount; i++)
                {
                    cardArray[i] = createCardByCardData(
                        filterCards[Random.Range(0, filterCards.Length)],
                        shouldInitialize);
                }

                return cardArray;
            }

            return Array.Empty<TMCardModel>();
        }

        public bool TryCreateCardByKey(string key, out TMCardModel card, bool shouldInitialize = true)
        {
            card = null;
            TMCardData cardData = TMCardDataManager.Instance.CardDataList.FirstOrDefault(cardData => cardData.CardKey == key);

            if (cardData)
            {
                card = createCardByCardData(cardData, shouldInitialize);
                return true;
            }

            return false;
        }

        public TMCardModel CreateRandomCard(bool shouldInitialize = true)
        {
            return TMCardDataManager.Instance.CardDataList.Count > 0 ?
                createCardByCardData(
                    TMCardDataManager.Instance.CardDataList[Random.Range(0, TMCardDataManager.Instance.CardDataList.Count)],
                    shouldInitialize) :
                null;
        }

        public TMCardModel CreateRandomCardByWhere(Func<TMCardData, bool> predicate, bool shouldInitialize = true)
        {
            TMCardData[] filterCards = TMCardDataManager
                .Instance
                .CardDataList
                .Where(predicate)
                .ToArray();

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
            TMCardModel card = PrototypeManager.Instance.ClonePrototypeFromReferenceSync<TMCardModel>(_templateReference);

            _onPreCreateCard.Invoke(card);

            card.SetCardData(cardData);

            if (shouldInitialize)
            {
                card.Initialize();
            }

            _onPostCreateCard.Invoke(card);

            return card;
        }
    }
}