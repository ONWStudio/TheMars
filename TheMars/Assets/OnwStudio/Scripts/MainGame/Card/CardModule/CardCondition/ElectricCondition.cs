using UnityEngine;
namespace TM.AdditionalCondition
{
    public sealed class ElectricCondition : ITMCardAdditionalCondition
    {
        [field: SerializeField] public int Electric { get; internal set; } = 0;
        public bool AdditionalCondition => true;
    }
}