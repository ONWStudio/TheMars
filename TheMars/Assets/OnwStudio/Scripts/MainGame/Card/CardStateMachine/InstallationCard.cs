using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 설치
/// </summary>
[SerializeReferenceDropdownName("설치")]
public sealed class InstallationCard : CardStateMachine
{
    private static readonly Dictionary<string, int> _cardStack = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _cardStack.Clear();
    }

    public override void OnUseStarted<T>(T cardController)
    {
        if (!_cardStack.ContainsKey(cardController.CardData.Guid))
        {
            _cardStack.Add(cardController.CardData.Guid, 1);
        }
        else
        {
            _cardStack[cardController.CardData.Guid]++;
        }
    }

    public override void OnDrawEnded<T>(T cardController)
    {
        if (!_cardStack.ContainsKey(cardController.CardData.Guid) ||
            _cardStack[cardController.CardData.Guid] <= 0) return;

        cardController.CardData.UseCard(cardController.gameObject);
        _cardStack[cardController.CardData.Guid]--;
    }
}
