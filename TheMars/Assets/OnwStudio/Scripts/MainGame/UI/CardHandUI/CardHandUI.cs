using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubClassSelectorSpace;

/// <summary>
/// .. 카드 패 UI 들을 관리하는 클래스
/// 카드 패들의 움직임과 상호작용에 관한 관리를 하는 클래스 입니다
/// </summary>
[DisallowMultipleComponent]
public sealed class CardHandUI : MonoBehaviour
{
    private const int MAX_CARD = 10;

    [SerializeReference, SubClassSelector(typeof(ICardSorter))] private ICardSorter _cardSorter = null;

    [field: SerializeField] public TMCardUI[] CardHandUIs { get; private set; } = new TMCardUI[MAX_CARD];

    private void Start()
    {
        _cardSorter ??= new SectorForm
        {
            MaxAngle = 128f,
            HeightRatioFromWidth = 0.25f
        };

        RectTransform rectTransform = transform as RectTransform;

        for (int i = 0; i < MAX_CARD; i++)
        {
            TMCardUI tMCardUI = new GameObject().AddComponent<TMCardUI>();
            tMCardUI.SetCardUI(transform);

            CardHandUIs[i] = tMCardUI;
        }

        UniRxObserver.ObserveInfomation(
            this,
            selector => _cardSorter,
            sorter => sorter.SortCards(CardHandUIs, rectTransform));
    }

    /// <summary>
    /// .. 카드 패의 형태를 결정하는 UI의 세팅을 합니다
    /// </summary>
    /// <param name="cardSoter"> .. CardSorter는 카드를 어떤 형태로 정렬시킬지 정하는 인터페이스 입니다 셋터는 한번 호출후 다시 결정할 수 없습니다 </param>
    public void SetHandUI(ICardSorter cardSoter)
    {
        _cardSorter ??= cardSoter;
    }
}
