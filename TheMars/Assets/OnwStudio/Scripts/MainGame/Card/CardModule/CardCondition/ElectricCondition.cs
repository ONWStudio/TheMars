using UnityEngine;
namespace TMCard.AddtionalCondition
{
    public sealed class ElectricCondition : ITMCardAddtionalCondition
    {
        [field: SerializeField] public int Electric { get; internal set; } = 0;
        public bool AdditionalCondition => true;
    }
}