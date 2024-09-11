// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Serialization;
// namespace TM.Card.Runtime
// {
//     // .. TODO : 카드 생성자 새로 만들기
//     /// <summary>
//     /// .. 덱에 있는 카드들을 관리하는 클래스 덱 컨트롤러 -> HandImporter -> 패 컨트롤러로 카드를 가져갑니다
//     /// </summary>
//     [DisallowMultipleComponent]
//     public sealed class TMCardDeckController : MonoBehaviour
//     {
//         public int CardCount => _cards.Count;
//
//         [FormerlySerializedAs("cards")]
//         [SerializeField]
//         private List<TMCardModel> _cards = new();
//
//         /// <summary>
//         /// .. 카드를 List의 형태로 받아옵니다
//         /// </summary>
//         /// <param name="count"> .. 받아올 카드 개수 덱에 카드가 부족한 경우 가지고 있는 만큼만 반환합니다 </param>
//         /// <returns> .. 덱에서 반환된 카드들 </returns>
//         public List<TMCardModel> DequeueCards(int count)
//         {
//             count = Mathf.Clamp(count, 0, 10);
//             List<TMCardModel> someCards = new(count);
//
//             while (someCards.Count < count && _cards.Count > 0)
//             {
//                 int index = Random.Range(0, _cards.Count);
//
//                 someCards.Add(_cards[index]);
//                 _cards.RemoveAt(index);
//             }
//
//             return someCards;
//         }
//
//         /// <summary>
//         /// .. 덱에 카드들을 넘겨줍니다
//         /// </summary>
//         /// <param name="cards"> .. 넘겨줄 카드 리스트 </param>
//         public void PushCards(List<TMCardModel> cards)
//         {
//             cards.ForEach(card => card.transform.SetParent(transform, false));
//             _cards.AddRange(cards);
//         }
//     }
// }
//     