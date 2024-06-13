using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCardUISystemModules
{
    [DisallowMultipleComponent]
    public sealed class TMCardTombUIController : MonoBehaviour
    {
        public int CardCount => _deadCards.Count;

        [SerializeField] private List<TMCardUIController> _deadCards = new();

        public void EnqueueDeadCards(List<TMCardUIController> cards)
        {
            cards.ForEach(card => card.transform.SetParent(transform, false));
            _deadCards.AddRange(cards);
        }

        public void EnqueueDeadCard(TMCardUIController card)
        {
            card.transform.SetParent(transform, false);
            _deadCards.Add(card);
        }

        public List<TMCardUIController> DequeueDeadCards()
        {
            List<TMCardUIController> deadCards = _deadCards.ToList();
            _deadCards.Clear();

            return deadCards;
        }
    }
}