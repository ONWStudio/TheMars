using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

/// <summary>
/// .. 카드의 랜덤
/// </summary>
public sealed class TMCardData : ScriptableObject
{
    /// <summary>
    /// .. 카드의 고유 ID 또는 스택 ID
    /// </summary>
    [field: SerializeField, Tooltip("카드의 고유 ID"), ReadOnly]
    public string Guid { get; private set; } = System.Guid.NewGuid().ToString();
    /// <summary>
    /// .. 마르스 리튬
    /// </summary>
    [field: SerializeField, Tooltip("소모 재화")]
    public int MarsLithium { get; private set; } = 0;
    /// <summary>
    /// .. 카드의 종류
    /// </summary>
    [field: SerializeField, Tooltip("카드의 종류")]
    public CARD_KIND CardKind { get; private set; }
    /// <summary>
    /// .. 카드의 등급
    /// </summary>
    [field: SerializeField, Tooltip("카드의 등급")]
    public CARD_GRADE CardGrade { get; private set; }
    /// <summary>
    /// .. 카드의 특수효과입니다
    /// CardStateMachine는 ITMCardController 상속받은 실제 카드 구현체를 바인딩하여 필요한 기능을 구현합니다
    /// </summary>
    [field: Space]
    [field: SerializeReference, InspectorName("특수 효과"), Tooltip("특수 효과"), SerializeReferenceDropdown]
    public CardStateMachine StateMachine { get; private set; } = new();
    /// <summary>
    /// .. 카드의 고유 이름
    /// </summary>
    public string CardName
        => _cardNames.TryGetValue(CultureInfo.CurrentCulture.Name, out string name) ? name : string.Empty;
    /// <summary>
    /// .. 카드 설명
    /// </summary>
    public string Description
        => _descriptions.TryGetValue(CultureInfo.CurrentCulture.Name, out string description) ? description : string.Empty;

    /// <summary>
    /// .. 읽기 전용 카드 이름 딕셔너리 각 지역별 이름을 반환합니다
    /// </summary>
    public IReadOnlyDictionary<string, string> ReadOnlyCardNames => _cardNames;
    /// <summary>
    /// .. 읽기 전용 카드 설명 딕셔너리 각 지역별 설명을 반환합니다
    /// </summary>
    public IReadOnlyDictionary<string, string> ReadOnlyDescriptions => _descriptions;
    /// <summary>
    /// .. 읽기 전용 카드 발동효과 리스트
    /// </summary>
    public IReadOnlyList<ICardEffect> ReadOnlyCardEffects => _cardEffects;
    /// <summary>
    /// .. 읽기 전용 카드 추가조건 리스트
    /// </summary>
    public IReadOnlyList<ICardCondition> ReadOnlyAdditionalCondition => _addtionalConditions;

    [Space]
    [SerializeField, Tooltip("카드 이름 관리자 지역별 이름을 작성합니다 로컬라이징은 국가 코드를 참고해주세요"), SerializedDictionary("Culture Code", "Name")]
    private SerializedDictionary<string, string> _cardNames = new() { { "en-US", "" }, { "ko-KR", "" } };

    [SerializeField, Tooltip("카드 설명 관리자 지역별 설명을 작성합니다 로컬라이징은 국가 코드를 참고해주세요"), SerializedDictionary("Culture Code", "Description")]
    private SerializedDictionary<string, string> _descriptions = new() { { "en-US", ""}, { "ko-KR", "" } };

    [SerializeReference, Tooltip("카드 발동 효과 리스트"), SerializeReferenceDropdown]
    private List<ICardEffect> _cardEffects = new();

    [SerializeReference, Tooltip("카드 추가 조건 리스트"), SerializeReferenceDropdown]
    private List<ICardCondition> _addtionalConditions = new();

    /// <summary>
    /// .. 발동 효과 입니다
    /// </summary>
    /// <param name="gameObject"></param>
    public void UseCard(GameObject gameObject)
    {
        _cardEffects.ForEach(cardEffect => cardEffect.OnEffect(gameObject, this));
    }

    /// <summary>
    /// .. 카드의 사용 전 카드가 사용가능한 상태인지 확인합니다
    /// </summary>
    /// <param name="marsLithium"> .. 현재 재화량 </param>
    /// <returns></returns>
    public bool IsAvailable(int marsLithium)
    {
        return MarsLithium <= marsLithium &&
            _addtionalConditions.All(additionalCondition => additionalCondition.AdditionalCondition);
    }

    [InspectorButton("Generate GUID")]
    private void generateNewGUID()
    {
        Guid = System.Guid.NewGuid().ToString();
    }
}