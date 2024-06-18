using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using SubClassSelectorSpace;

/// <summary>
/// .. 카드의 랜덤
/// </summary>
public sealed class TMCardData : ScriptableObject
{
    /// <summary>
    /// .. 카드의 고유 ID 또는 스택 ID
    /// </summary>
    [field: SerializeField, ReadOnly] public string Guid { get; private set; }
    /// <summary>
    /// .. 마르스 리튬
    /// </summary>
    [field: SerializeField] public int MarsLithium { get; private set; } = 0;
    /// <summary>
    /// .. 카드의 종류
    /// </summary>
    [field: SerializeField] public CARD_KIND CardKind { get; private set; }
    /// <summary>
    /// .. 카드의 등급
    /// </summary>
    [field: SerializeField] public CARD_GRADE CardGrade { get; private set; }
    /// <summary>
    /// .. 카드의 특수효과입니다
    /// CardStateMachine는 ITMCardController 상속받은 실제 카드 구현체를 바인딩하여 필요한 기능을 구현합니다
    /// </summary>
    [field: SerializeReference, SubClassSelector(typeof(CardStateMachine))] public CardStateMachine StateMachine { get; private set; } = new();
    /// <summary>
    /// .. 카드의 고유 이름
    /// </summary>
    public string CardName => _cardNames.TryGetValue(CultureInfo.CurrentCulture.Name, out string name) ? name : string.Empty;
    /// <summary>
    /// .. 카드 설명
    /// </summary>
    public string Description => _descriptions.TryGetValue(CultureInfo.CurrentCulture.Name, out string description) ? description : string.Empty;

    public IReadOnlyDictionary<string, string> ReadOnlyCardNames => _cardNames;
    public IReadOnlyDictionary<string, string> ReadOnlyDescriptions => _descriptions;

    public IReadOnlyList<ICardEffect> ReadOnlyCardEffects => _cardEffects;
    public IReadOnlyList<ICardCondition> ReadOnlyAdditionalCondition => _addtionalConditions;

    [SerializeField, SerializedDictionary("Culture Code", "Name")] private SerializedDictionary<string, string> _cardNames = new() { { "en-US", "" }, { "ko-KR", "" } };
    [SerializeField, SerializedDictionary("Culture Code", "Description")] private SerializedDictionary<string, string> _descriptions = new() { { "en-US", ""}, { "ko-KR", "" } };
    [SerializeField] private List<ICardEffect> _cardEffects = new();
    [SerializeField] private List<ICardCondition> _addtionalConditions = new();

    /// <summary>
    /// .. 발동 효과 입니다
    /// </summary>
    /// <param name="gameObject"></param>
    public void UseCard(GameObject gameObject)
    {
        _cardEffects.ForEach(cardEffect => cardEffect.OnEffect(gameObject, this));
    }

    public bool IsAvailable(int marsLithium)
    {
        return MarsLithium <= marsLithium &&
            _addtionalConditions.All(additionalCondition => additionalCondition.AdditionalCondition);
    }

    [Button("Generate GUID")]
    private void GenerateNewGUID()
    {
        Guid = System.Guid.NewGuid().ToString();
    }
}