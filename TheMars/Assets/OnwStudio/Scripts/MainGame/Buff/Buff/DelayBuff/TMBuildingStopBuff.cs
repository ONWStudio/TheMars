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
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public sealed class TMBuildingStopBuff : TMDelayBuff, ITMInitializeBuff<TMBuildingStopBuffTrigger>
    {
        private TMBuilding[] _stoppedBuildings = null;

        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference = new("BuildingStopped");
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "Building_Stop_Buff");

        public override Color IconBackgroundColor => ColorUtility.TryParseHtmlString("#8E3214", out Color red) ? red : Color.red;
        public override AssetReferenceSprite IconReference => _iconReference;
        [field: SerializeField, ReadOnly] public int TargetBuildingCount { get; private set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMBuildingStopBuffTrigger creator)
        {
            base.Initialize(creator);
            TargetBuildingCount = creator.TargetBuildingCount;

            _description.Arguments = new object[]
            {
                new
                {
                    DelayDayCount,
                    IsTemporary,
                    TargetBuildingCount
                }
            };
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
            _stoppedBuildings = TMGridManager
                .Instance
                .Buildings
                .OrderBy(_ => Random.value)
                .Take(TargetBuildingCount)
                .ToArray();

            _stoppedBuildings.ForEach(building => building.IsActive.Value = false);
        }
        
        protected override void OnChangedDayByDelayCount(int day)
        {
            _stoppedBuildings.ForEach(building => building.IsActive.Value = true);
        }
    }
}
