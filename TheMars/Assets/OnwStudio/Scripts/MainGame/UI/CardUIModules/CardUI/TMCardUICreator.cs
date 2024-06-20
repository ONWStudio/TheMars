using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMCardUISystemModules
{
    public static class TMCardUICreator
    {
        public static List<TMCardUIController> CreateCards(int createCount)
        {
            List<TMCardUIController> cardList = new(createCount);

            for (int i = 0; i < createCount; i++)
            {
                cardList.Add(new GameObject($"Card_{i + 1}").AddComponent<TMCardUIController>());
                // .. TODO : Ä«µå ·£´ý »Ì±â
                cardList[i].InitializeCard(null);
            }

            return cardList;
        }

        public static TMCardUIController CreateCard()
        { 
            TMCardUIController cardUI = new GameObject("Card").AddComponent<TMCardUIController>();
            cardUI.InitializeCard(null);

            return cardUI;
        }
    }
}