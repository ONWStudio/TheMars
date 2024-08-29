// using Onw.Attribute;
// using Onw.ServiceLocator;
// using TMCard.Runtime;
// using UnityEngine;
// namespace TMCard.Effect
// {
//     /// <summary>
//     /// .. 지속 (시간)
//     /// </summary>
//     public sealed class TimeContinuingEffect : TMCardSpecialEffect, ITMCardInitializeEffect<TimeContinuingEffectCreator>
//     {
//         /// <summary>
//         /// .. 지속 시간
//         /// </summary>
//         [field: SerializeField, ReadOnly] public float ContinuingTime { get; private set; } = 1f;
//
//         public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
//         {
//             controller.OnClickEvent.RemoveAllToAddListener(eventState => onContinuingSeconds(
//                 controller,
//                 ContinuingTime,
//                 () => trigger.OnEffectEvent.Invoke(eventState)));
//
//             static void onContinuingSeconds(TMCardController card, float continuingSeconds, System.Action onSuccess)
//             {
//                 if (!ServiceLocator<ITMCardService>.TryGetService(out ITMCardService service) ||
//                     !ServiceLocator<TMDelayEffectManager>.TryGetService(out TMDelayEffectManager delayEffectManager)) return;
//
//                 delayEffectManager.WaitForSecondsEffect(
//                     continuingSeconds,
//                     () =>
//                     {
//                         Debug.Log("지속 시간 종료");
//                         onSuccess.Invoke();
//                     },
//                     remainingTime => { });
//
//                 card.MoveToTombAndHide();
//             }
//         }
//
//         public void Initialize(TimeContinuingEffectCreator effectCreator)
//         {
//             ContinuingTime = effectCreator.ContinuingTime;
//         }
//
//         public TimeContinuingEffect() : base("TimeContinuing") {}
//     }
// }