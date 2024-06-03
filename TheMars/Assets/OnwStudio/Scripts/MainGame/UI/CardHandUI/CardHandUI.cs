using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UITools.UITool;

// .. 카드 정렬 형태에 따로 인터페이스 상속구현
public sealed class CardHandUI : MonoBehaviour
{
    private const int MAX_CARD = 10;
    private ICardSorter _cardSorter = new SectorForm();

    [field: SerializeField] public TMCardUI[] CardHandUIs { get; private set; } = new TMCardUI[MAX_CARD];

    private void Start()
    {
        RectTransform rectTransform = transform as RectTransform;

        for (int i = 0; i < MAX_CARD; i++)
        {
            TMCardUI tMCardUI = new GameObject().AddComponent<TMCardUI>();
            tMCardUI.SetCardUI(transform, tMCardUI.gameObject.AddComponent<SmoothMove>());

            CardHandUIs[i] = tMCardUI;
        }

        _cardSorter.SortCards(
            CardHandUIs.Select(cardUI => cardUI.CardMovement).ToArray(),
            new(0f, -(rectTransform.rect.height * 0.5f)),
            rectTransform.rect.width * 0.5f);
    }
}
