using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Extensions;

namespace TMCard.Runtime
{
    /// <summary>
    /// .. 다음턴에 꺼낼 카드들을 패에 보내기전에 임포터 클래스를 거쳐갑니다.
    /// 해당 클래스를 거치는 이유는 카드 효과중 다음턴에 다시 돌려쓰는 카드가 있을 수 있기 때문입니다
    /// </summary>
    [System.Serializable]
    public sealed class TMCardHandImporter
    {
        public int Stack => _cardQueue.Count;

        [SerializeField]
        private Queue<TMCardController> _cardQueue = new(10);

        public void PushCards(IEnumerable<TMCardController> cards)
            => cards.ForEach(_cardQueue.Enqueue);

        public void PushCard(TMCardController card)
            => _cardQueue.Enqueue(card);

        public List<TMCardController> GetCards(int count)
        {
            List<TMCardController> cards = new(count);
            count = Mathf.Clamp(count, 0, 10);

            for (int i = 0; i < count; i++)
            {
                if (_cardQueue.Count <= 0 || cards.Count >= count) break;

                cards.Add(_cardQueue.Dequeue());
            }

            return cards;
        }
    }
}
