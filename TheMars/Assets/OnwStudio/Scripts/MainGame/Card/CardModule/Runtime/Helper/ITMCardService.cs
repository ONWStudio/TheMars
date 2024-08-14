using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMCard.Runtime
{
    public interface ITMCardService
    {
        int HandCardCount { get; }
        int DeckCardCount { get; }
        int TombCardCount { get; }
        int AnimatedCardCount { get; } 
        // .. 패
        TMCardHandController CardHandController { get; }
        // .. 덱
         TMCardDeckController CardDeckController { get; }
        // .. 무덤
         TMCardTombController CardTombController { get; }
         UnityEvent OnTurnEnd { get; }
         UnityEvent<TMCardController> OnUsedCard { get; }
         Camera CardSystemCamera { get; }
         TMCardCreator CardCreator { get; }

         void AddListenerToCard(TMCardController card);
    }
}