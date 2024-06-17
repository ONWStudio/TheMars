using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InstallationCard : CardStateMachine
{
    private static readonly Dictionary<int, int> _cardStack = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _cardStack.Clear();
    }

    public override void OnUseStarted<T>(T cardController)
    {
        if (!_cardStack.ContainsKey(cardController.CardData.No))
        {
            _cardStack.Add(cardController.CardData.No, 1);
        }
        else
        {
            _cardStack[cardController.CardData.No]++;
        }
    }

    public override void OnDrawEnded<T>(T cardController)
    {
        if (!_cardStack.ContainsKey(cardController.CardData.No) ||
            _cardStack[cardController.CardData.No] <= 0) return;

        cardController.CardData.UseCard(cardController.gameObject);
        _cardStack[cardController.CardData.No]--;
    }
}
