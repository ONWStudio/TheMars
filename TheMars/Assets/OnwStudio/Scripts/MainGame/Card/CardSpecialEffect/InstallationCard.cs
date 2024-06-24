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

    public void ApplyEffect<T>(T cardController) where T : MonoBehaviour, ITMCardController<T>
    {
        throw new System.NotImplementedException();
    }

    public bool CanCoexistWith(IEnumerable<ICardSpecialEffect> cardSpecialEffects)
    {
        throw new System.NotImplementedException();
    }
}
