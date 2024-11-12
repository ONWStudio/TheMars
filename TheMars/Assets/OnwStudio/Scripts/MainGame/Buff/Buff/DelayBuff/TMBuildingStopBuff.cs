using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using Onw.Extensions;
using TM.Grid;
using TM.Manager;
using TM.Building;
using TM.Buff.Trigger;

namespace TM.Buff
{
    [System.Serializable]
    public sealed class TMBuildingStopBuff : TMDelayBuff, ITMInitializeBuff<TMBuildingStopBuffTrigger>
    {
        [field: SerializeField, ReadOnly] protected override AssetReferenceSprite IconReference { get; set; }
        [field: SerializeField, ReadOnly] public int TargetBuildingCount { get; private set; }

        private TMBuilding[] _stoppedBuildings = null;
        
        public void Initialize(TMBuildingStopBuffTrigger creator)
        {
            base.Initialize(creator);
            TargetBuildingCount = creator.TargetBuildingCount;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _stoppedBuildings = null;
            }
        }
        
        protected override void OnApplyBuff()
        {
            _stoppedBuildings = TMGridManager.Instance.Buildings.OrderBy(_ => Random.value).Take(TargetBuildingCount).ToArray();
            _stoppedBuildings.ForEach(building => building.IsActive.Value = false);
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
            _stoppedBuildings.ForEach(building => building.IsActive.Value = true);
        }
    }
}
