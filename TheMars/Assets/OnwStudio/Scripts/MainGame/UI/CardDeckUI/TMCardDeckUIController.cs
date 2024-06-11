using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCardUISystemModules
{
    // .. TODO : 카드 생성자 새로 만들기
    /// <summary>
    /// .. 덱에 있는 카드들을 관리하는 클래스 덱 컨트롤러 -> HandImporter -> 패 컨트롤러로 카드를 가져갑니다
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TMCardDeckUIController : MonoBehaviour
    {
        private const int CARD_MAX = 50;

        public int CardCount => _cards.Count;

        [SerializeField] private List<TMCardUIController> _cards = new(CARD_MAX);

        private void Awake()
        {
            _cards.AddRange(TMCardUICreator.Instance.CreateCards(CARD_MAX));
            _cards.ForEach(card => card.SetCardUI(transform));
        }

        public List<TMCardUIController> GetCards(int count)
        {
            count = Mathf.Clamp(count, 0, 10);
            List<TMCardUIController> someCards = new(count);

            while (someCards.Count < count && _cards.Count > 0)
            {
                int index = Random.Range(0, _cards.Count);

                someCards.Add(_cards[index]);
                _cards.RemoveAt(index);
            }

            return someCards;
        }
    }
}
    