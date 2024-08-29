// using System.Collections.Generic;
// using System.Linq;
// using TMCard.Runtime;
// namespace TMCard.Effect
// {
//     /// <summary>
//     /// .. 드로우
//     /// </summary>
//     public sealed class DrawEffect : TMCardSpecialEffect, ITMCardInitializeEffect<DrawEffectCreator>, ITMEffectTrigger, ITMInnerEffector
//     {
//         public CardEvent OnEffectEvent { get; } = new();
//         public IReadOnlyList<ITMNormalEffect> InnerEffect => _drawEffects;
//
//         private readonly List<ITMNormalEffect> _drawEffects = new();
//
//         public void Initialize(DrawEffectCreator effectCreator)
//         {
//             _drawEffects.AddRange(effectCreator.DrawEffects);
//         }
//
//         public override void ApplyEffect(TMCardController controller, ITMEffectTrigger trigger)
//         {
//             _drawEffects.ForEach(effect => effect.ApplyEffect(controller, this));
//             controller.OnDrawEndedEvent.RemoveAllToAddListener(OnEffectEvent.Invoke);
//         }
//
//         public DrawEffect() : base("Draw") { }
//     }
// }
