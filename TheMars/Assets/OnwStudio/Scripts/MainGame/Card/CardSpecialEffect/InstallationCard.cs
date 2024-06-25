using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// .. 설치
/// </summary>
[SerializeReferenceDropdownName("설치")]
public sealed class InstallationCard : ICardSpecialEffect
{
    private static readonly Dictionary<string, int> _cardStack = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void initialize()
    {
        _cardStack.Clear();
    }

    public void ApplyEffect<T>(T cardController) where T : TMCardController<T>
    {
        cardController.UseStartedState = () => cardController.OnMoveToScreenCenter.Invoke(cardController);
        cardController.DrawEndedState = () => onDraw(cardController);
    }

    private void onDraw<T>(T cardController) where T : TMCardController<T>
    {
        if (!_cardStack.ContainsKey(cardController.CardData.CardName))
        {
            _cardStack.Add(cardController.CardData.CardName, 0);
        }

        _cardStack[cardController.CardData.CardName]++;

        if (_cardStack[cardController.CardData.CardName] > 1)
        {
            cardController.DrawEndedState = () =>
            {
                cardController.CardData.UseCard(cardController.gameObject);
                cardController.OnMoveToScreenCenter.Invoke(cardController);
            };
        }
    }
}
