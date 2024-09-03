// using System;
// namespace TMCard
// {
//     /// <summary>
//     /// .. 현재 이벤트가 어떤 경우에 호출되었는지 리스너들에게 알려줍니다
//     /// 이벤트의 종류가 어떤 것인지에 따라 리스너가 다른 처리를 할 수 있게 설계할 수 있습니다
//     /// 예 => NORMAL(일반 사용) 일때 트리거 시 재화획득 TURN_END 일때 트리거 시 재화손실
//     /// </summary>
//
//     public sealed class CardEvent
//     {
//         private Action _cardAction;
//
//         public void Invoke()
//         {
//             _cardAction?.Invoke();
//         }
//
//         public void AddListener(Action action)
//         {
//             if (_cardAction is null)
//             {
//                 _cardAction = action;
//                 return;
//             }
//
//             _cardAction += action;
//         }
//
//         public void RemoveAllToAddListener(Action action)
//         {
//             _cardAction = action;
//         }
//
//         public void RemoveListener(Action action)
//         {
//             if (_cardAction is null) return;
//
//             _cardAction -= action;
//         }
//
//         public void RemoveAllListener()
//         {
//             _cardAction = null;
//         }
//     }
// }