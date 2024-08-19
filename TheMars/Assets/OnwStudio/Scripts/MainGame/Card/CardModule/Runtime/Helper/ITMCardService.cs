using UnityEngine;
using UnityEngine.Events;
using Onw.Feedback;
using Onw.Event;
using TMCard.UI;

namespace TMCard.Runtime
{

    
    public interface ITMCardService : IFeedbackPlayer
    {
        int HandCardCount { get; }
        int DeckCardCount { get; }
        int TombCardCount { get; }
        bool IsAnimated { get; } 
        // .. 패
        TMCardHandController CardHandController { get; }
        // .. 덱
         TMCardDeckController CardDeckController { get; }
        // .. 무덤
         TMCardTombController CardTombController { get; }
         
         TMCardCollectUI CollectUI { get; }
         SafeUnityEvent OnTurnEnd { get; }
         SafeUnityEvent<TMCardController> OnUsedCard { get; }
         Camera CardSystemCamera { get; }
         TMCardCreator CardCreator { get; }

         void AddListenerToCard(TMCardController card);
         void OnClickCard(TMCardController card);
    }
}