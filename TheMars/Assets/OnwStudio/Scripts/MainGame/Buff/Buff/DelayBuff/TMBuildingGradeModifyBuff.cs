using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Extensions;
using TM.Buff.Trigger;
using TM.Building;
using TM.Grid;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMBuildingGradeModifyBuff : TMDelayBuff, ITMInitializeBuff<TMBuildingGradeModifyBuffTrigger>
    {
        private TMBuilding[] _targetBuildings = null;
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference;
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Building_Grade_Modify_Buff");

        public override AssetReferenceSprite IconReference => _iconReference;
        [field: SerializeField] public TMBuildingKindForWhere Kind { get; set; }
        [field: SerializeField] public int TargetBuildingCount { get; set; }
        [field: SerializeField] public int GradeUpValue { get; set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMBuildingGradeModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            Kind = creator.Kind;
            TargetBuildingCount = creator.TargetBuildingCount;
            GradeUpValue = creator.GradeUpValue;
            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    Kind,
                    TargetBuildingCount,
                    GradeUp = Mathf.Abs(GradeUpValue),
                    Positive = GradeUpValue >= 0
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _targetBuildings = null;
            }
        }
        
        protected override void OnApplyBuff()
        {
            _targetBuildings = TMGridManager
                .Instance
                .Buildings
                .Where(isEqualKind)
                .OrderBy(_ => Random.value)
                .Take(TargetBuildingCount)
                .ToArray();

            _targetBuildings.ForEach(building => building.GradePlus.Value += GradeUpValue);

            bool isEqualKind(TMBuilding building)
            {
                return Kind switch
                {
                    TMBuildingKindForWhere.PIONEER => building.BuildingData.Kind == TMBuildingKind.PIONEER,
                    TMBuildingKindForWhere.RESOURCEFUL => building.BuildingData.Kind == TMBuildingKind.RESOURCEFUL,
                    TMBuildingKindForWhere.BUILDER => building.BuildingData.Kind == TMBuildingKind.BUILDER,
                    TMBuildingKindForWhere.EXTERNAL => building.BuildingData.Kind == TMBuildingKind.EXTERNAL,
                    _ => true,
                };
            }
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
            _targetBuildings.ForEach(building => building.GradePlus.Value -= GradeUpValue);
        }
    }
}
