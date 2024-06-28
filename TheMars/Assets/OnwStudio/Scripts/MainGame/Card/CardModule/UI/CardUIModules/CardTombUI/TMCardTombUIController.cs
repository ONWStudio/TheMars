using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCard.UI
{
    [DisallowMultipleComponent]
    public sealed class TMCardTombUIController : MonoBehaviour
    {
        public int CardCount => _deadCards.Count;

        [SerializeField] private List<TMCardController> _deadCards = new();

        public void EnqueueDeadCards(List<TMCardController> cards)
        {
            cards.ForEach(card => card.transform.SetParent(transform, false));
            _deadCards.AddRange(cards);
        }

        public void EnqueueDeadCard(TMCardController card)
        {
            card.transform.SetParent(transform, false);
            _deadCards.Add(card);
        }

        public List<TMCardController> DequeueDeadCards()
        {
            List<TMCardController> deadCards = _deadCards.ToList();
            _deadCards.Clear();

            return deadCards;
        }
    }
}