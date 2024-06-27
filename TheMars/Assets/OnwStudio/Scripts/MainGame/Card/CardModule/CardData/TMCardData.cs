using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using AYellowpaper.SerializedCollections;
using OnwAttributeExtensions;

public sealed partial class TMCardData : ScriptableObject
{
    /// <summary>
    /// .. 카드의 고유 ID
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<Guid>k__BackingField"), Tooltip("카드의 고유 ID"), ReadOnly]
    public string Guid { get; private set; } = System.Guid.NewGuid().ToString();

    /// <summary>
    /// .. 카드 전체와 공유하는 스택 ID
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<StackID>k__BackingField"), Tooltip("Stack ID")]
    public int StackID { get; private set; } = 0;

    /// <summary>
    /// .. 같은 그룹간에 공유하는 스택 ID
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<GroupStackID>k__BackingField"), Tooltip("그룹 Stack ID")]
    public int GroupStackID { get; private set; } = 0;

    /// <summary>
    /// .. 테라
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<Tera>k__BackingField"), DisplayAs("소모 재화 (테라)"), Tooltip("테라")]
    public int Tera { get; private set; } = 0;

    /// <summary>
    /// .. 마르스 리튬
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<MarsLithium>k__BackingField"), DisplayAs("소모 재화 (마르스 리튬)"), Tooltip("마르스 리튬")]
    public int MarsLithium { get; private set; } = 0;

    /// <summary>
    /// .. 카드의 종류
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<CardKind>k__BackingField"), DisplayAs("카드 종류"), Tooltip("카드의 종류")]
    public TM_CARD_KIND CardKind { get; private set; } = TM_CARD_KIND.CONSTRUCTION;
    /// <summary>
    /// .. 카드의 등급
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<CardGrade>k__BackingField"), DisplayAs("등급"), Tooltip("카드의 등급")]
    public TM_CARD_GRADE CardGrade { get; private set; } = TM_CARD_GRADE.NORMAL;

    /// <summary>
    /// .. 카드의 그룹
    /// </summary>
    [field: SerializeField, FormerlySerializedAs("<CardGroup>k__BackingField"), DisplayAs("그룹"), Tooltip("카드의 그룹")]
    public TM_CARD_GROUP CardGroup { get; private set; } = TM_CARD_GROUP.COMMON;

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

    public IReadOnlyDictionary<string, string> ReadOnlyCardNames => _cardNames;
    public IReadOnlyDictionary<string, string> ReadOnlyDescriptions => _descriptions;
    public IReadOnlyList<ICardCondition> ReadOnlyAdditionalCondition => _addtionalConditions;
    public IReadOnlyList<ICardSpecialEffect> SpecialEffects => _specialEffect;

    /// <summary>
    /// .. 카드의 특수효과입니다
    /// ICardSpecialUseStart는 ITMCardController 상속받은 실제 카드 구현체를 바인딩하여 필요한 기능을 구현합니다
    /// </summary>
    [Space]
    [SerializeReference, FormerlySerializedAs("_specialEffect"), DisplayAs("특수 효과"), Tooltip("특수 효과"), SerializeReferenceDropdown]
    private List<ICardSpecialEffect> _specialEffect = new();

    [Space]
    [SerializeField, FormerlySerializedAs("_cardNames"), DisplayAs("이름"), Tooltip("카드 이름 관리자 지역별 이름을 작성합니다 로컬라이징은 국가 코드를 참고해주세요"), SerializedDictionary("Culture Code", "Name")]
    private SerializedDictionary<string, string> _cardNames = new() { { "en-US", "" }, { "ko-KR", "" } };

    [SerializeField, FormerlySerializedAs("_descriptions"), DisplayAs("설명"), Tooltip("카드 설명 관리자 지역별 설명을 작성합니다 로컬라이징은 국가 코드를 참고해주세요"), SerializedDictionary("Culture Code", "Description")]
    private SerializedDictionary<string, string> _descriptions = new() { { "en-US", "" }, { "ko-KR", "" } };

    [SerializeReference, FormerlySerializedAs("_cardEffects"), DisplayAs("발동 효과"), Tooltip("카드 발동 효과 리스트"), SerializeReferenceDropdown]
    private List<ITMCardEffect> _cardEffects = new();

    [SerializeReference, FormerlySerializedAs("_addtionalConditions"), DisplayAs("추가 조건"), Tooltip("카드 추가 조건 리스트"), SerializeReferenceDropdown]
    private List<ICardCondition> _addtionalConditions = new();

    /// <summary>
    /// .. 발동 효과 입니다
    /// </summary>
    public void UseCard()
    {
        _cardEffects.ForEach(cardEffect => cardEffect.OnEffect(this));
    }

    public IEnumerable<T> GetEffectOfType<T>() where T : ITMCardEffect
    {
        return _cardEffects.OfType<T>();
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
}