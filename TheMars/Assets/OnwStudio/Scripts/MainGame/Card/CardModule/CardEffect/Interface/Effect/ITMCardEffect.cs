using UnityEngine.Localization;
using Onw.Interface;
using TM.Card.Runtime;

namespace TM.Card.Effect
{
    public interface ITMCardEffect
    {
        public LocalizedString LocalizedDescription { get; }
        bool CanUseEffect { get;}
        /// <summary>
        /// .. 카드 사용시
        /// </summary>
        void ApplyEffect(TMCardModel cardModel);
        void OnEffect(TMCardModel cardModel);
    }
}