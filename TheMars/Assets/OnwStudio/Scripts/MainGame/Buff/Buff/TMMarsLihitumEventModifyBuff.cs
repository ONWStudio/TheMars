using Onw.Attribute;
using Onw.Event;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TM.Buff.Trigger;
using TM.Event;
using TM.Cost;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace TM.Buff
{
    [System.Serializable]
    public class TMMarsLihitumEventModifyBuff : TMBuffBase, ITMInitializeBuff<TMMarsLithiumEventModifyBuffTrigger>, IRemainingCountNotifier
    {
        [SerializeField, ReadOnly] private AssetReferenceSprite _iconReference = new("MarsLithiumEventBuff");
        [SerializeField, ReadOnly] private LocalizedString _description = new("TM_Buff_Effect", "MarsLithium_Event_Modify_Buff");
        [SerializeField, ReadOnly] private ReactiveField<int> _remainingEndTriggerCount = new();

        public override Color IconBackgroundColor => ColorUtility.TryParseHtmlString("#898E14", out Color yellow) ? yellow : Color.yellow;
        public override AssetReferenceSprite IconReference => _iconReference;

        public IReadOnlyReactiveField<int> RemainingCount => _remainingEndTriggerCount;

        [field: SerializeField, ReadOnly] public int EndTriggerCount { get; private set; }
        [field: SerializeField, ReadOnly] public int AddMarsLithium { get; private set; }
        [field: SerializeField, ReadOnly] public bool IsTemporary { get; private set; }

        public override LocalizedString Description => _description;

        public void Initialize(TMMarsLithiumEventModifyBuffTrigger creator)
        {
            EndTriggerCount = creator.EndTriggerCount;
            AddMarsLithium = creator.AddMarsLithium;
            IsTemporary = creator.IsTemporay;

            _description.Arguments = new object[]
            {
                new
                {
                    IsTemporary,
                    EndTriggerCount,
                    MarsLithium = Mathf.Abs(AddMarsLithium),
                    Positive = AddMarsLithium >= 0
                }
            };
        }

        protected override void ApplyBuffProtected()
        {
            int eventCount = 0;
            _remainingEndTriggerCount.Value = EndTriggerCount - eventCount;

            TMMultiplyDayResourceCost marsLithiumCost = TMEventManager
                .Instance
                .MainEventRunner
                .TopCosts
                .OfType<TMMultiplyDayResourceCost>()
                .FirstOrDefault(cost => cost.Kind == TMResourceKind.MARS_LITHIUM);

            TMEventManager.Instance.OnTriggerEvent += onTriggerEvent;
            marsLithiumCost.AdditionalCost.Value += AddMarsLithium;

            void onTriggerEvent(ITMEventRunner runner)
            {
                if (runner.EventReadData.ID != TMEventDataManager.Instance.MarsLithiumEvent.ID) return;

                eventCount++;
                _remainingEndTriggerCount.Value = EndTriggerCount - eventCount;

                if (EndTriggerCount % eventCount == 0 && !IsTemporary)
                {
                    TMEventManager.Instance.OnTriggerEvent -= onTriggerEvent;
                    marsLithiumCost.AdditionalCost.Value -= AddMarsLithium;
                    Dispose();
                }
            }
        }
    }
}
