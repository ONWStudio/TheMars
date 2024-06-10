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
public sealed class TMCardHandUIController : MonoBehaviour
{
    public int CardCount => _cards.Count;

    [SerializeReference, SubClassSelector(typeof(ICardSorter))] private ICardSorter _cardSorter = null;
    [SerializeField] private List<TMCardUIController> _cards = new(10);

    private RectTransform _rectTransform = null;

    private void Awake()
    {
        _cardSorter ??= new SectorForm
        {
            MaxAngle = 128f,
            HeightRatioFromWidth = 0.25f
        };

        _rectTransform = transform as RectTransform;
    }

    public void SetCards(List<TMCardUIController> cards)
    {
        foreach (TMCardUIController card in cards)
        {
            card.SetCardUI(transform);
            card.SetOn();
            card.OnDestroyEvent.AddListener(cardUI => _cards.Remove(cardUI));
        }

        _cards.AddRange(cards);

        UniRxObserver.ObserveInfomation(
            this,
            selector => _cardSorter,
            sorter => sorter.SortCards(_cards, _rectTransform));
    }

    /// <summary>
    /// .. 카드 패의 형태를 결정하는 UI의 세팅을 합니다
    /// </summary>
    /// <param name="cardSoter"> .. CardSorter는 카드를 어떤 형태로 정렬시킬지 정하는 인터페이스 입니다 셋터는 한번 호출후 다시 결정할 수 없습니다 .. 인스펙터에서 변경 가능 </param>
    public void SetHandUI(ICardSorter cardSoter)
    {
        _cardSorter ??= cardSoter;
    }
}
