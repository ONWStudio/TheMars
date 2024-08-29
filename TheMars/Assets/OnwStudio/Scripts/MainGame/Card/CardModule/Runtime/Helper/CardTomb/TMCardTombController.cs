using System.Collections.Generic;
using System.Linq;
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
        private List<TMCardModel> _deadCards = new();

        public void EnqueueDeadCards(List<TMCardModel> cards)
        {
            cards.ForEach(card => card.transform.SetParent(transform, false));
            _deadCards.AddRange(cards);
        }

        public void EnqueueDeadCard(TMCardModel card)
        {
            card.transform.SetParent(transform, false);
            _deadCards.Add(card);
        }

        public List<TMCardModel> DequeueDeadCards()
        {
            List<TMCardModel> deadCards = _deadCards.ToList();
            _deadCards.Clear();

            return deadCards;
        }
    }
}