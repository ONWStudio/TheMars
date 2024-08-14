using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMCard.Runtime
{
    [DisallowMultipleComponent]
    public sealed class TMCardTombController : MonoBehaviour
    {
        public int CardCount => _deadCards.Count;

        [FormerlySerializedAs("deadCards")]
        [SerializeField]
        private List<TMCardController> _deadCards = new();

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
            List<TMCardController> deadCards = this._deadCards.ToList();
            this._deadCards.Clear();

            return deadCards;
        }
    }
}