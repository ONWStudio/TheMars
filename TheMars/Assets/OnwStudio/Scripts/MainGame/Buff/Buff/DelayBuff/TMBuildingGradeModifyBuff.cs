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

namespace TM.Buff
{
    [System.Serializable]
    public class TMBuildingGradeModifyBuff : TMDelayBuff, ITMInitializeBuff<TMBuildingGradeModifyBuffTrigger>
    {
        [field: SerializeField, ReadOnly] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField] public TMBuildingKindForWhere Kind { get; set; }
        [field: SerializeField] public int TargetBuildingCount { get; set; }
        [field: SerializeField] public int GradeUpValue { get; set; }

        private TMBuilding[] _targetBuildings = null;
        
        public void Initialize(TMBuildingGradeModifyBuffTrigger creator)
        {
            base.Initialize(creator);

            Kind = creator.Kind;
            TargetBuildingCount = creator.TargetBuildingCount;
            GradeUpValue = creator.GradeUpValue;
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
