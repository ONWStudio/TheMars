using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class TMCardGameManager : MonoBehaviour
{
    // .. 덱
    private TMCardDeckUIController _cardDeckUIController = null;
    // .. 패
    private TMCardHandUIController _cardHandUIController = null;
    // .. 무덤
    private TMCardTombUIController _cardTombUIController = null;

    [Header("Controller Object")]
    [SerializeField] private GameObject _deckUIObject = null;
    [SerializeField] private GameObject _handUIObject = null;
    [SerializeField] private GameObject _tombUIObject = null;

    // .. 임포터
    [Header("Card Importer")]
    [SerializeField] private TMCardHandImporter _cardHandImporter = null;

    private void Awake()
    {
        _cardDeckUIController = _deckUIObject.AddComponent<TMCardDeckUIController>();
        _cardHandUIController = _handUIObject.AddComponent<TMCardHandUIController>();
        _cardTombUIController = _tombUIObject.AddComponent<TMCardTombUIController>();
    }

    private void Start()
    {
        _cardHandImporter.PushCards(_cardDeckUIController.GetCards(10));
        DrawCardFromDeck();
    }

    public void DrawCardFromDeck()
    {
        int count = 10 - _cardHandUIController.CardCount;

        List<TMCardUIController> importerCard = _cardHandImporter.GetCards(count);

        importerCard.AddRange(_cardDeckUIController.GetCards(count - importerCard.Count));
        _cardHandUIController.SetCards(importerCard);
    }
}
