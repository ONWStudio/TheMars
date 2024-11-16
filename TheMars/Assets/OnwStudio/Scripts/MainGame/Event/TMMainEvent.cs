using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Attribute;
using TM.Event.Effect;
using TM.Usage;
using System.Linq;

namespace TM.Event
{
    public interface ITMEvent
    {
        event UnityAction<TMEventChoice> OnFireEvent;
        ITMEventData EventReadData { get; }
    }

    [System.Serializable]
    public sealed class TMEventRunner : ITMEvent
    {
        public event UnityAction<TMEventChoice> OnFireEvent
        {
            add => _onFireEvent.AddListener(value);
            remove => _onFireEvent.RemoveListener(value);
        }

        public ITMEventData EventReadData => EventData;
        public IReadOnlyList<ITMEventEffect> TopEffects => _topEffects;
        public IReadOnlyList<ITMEventEffect> BottomEffects => _bottomEffects;

        public IReadOnlyList<ITMUsage> TopUsages => _topUsages;
        public IReadOnlyList<ITMUsage> BottomUsages => _bottomUsages;

        [field: SerializeField] public TMEventData EventData { get; private set; }

        [SerializeField, ReadOnly] private List<ITMEventEffect> _topEffects = new();
        [SerializeField, ReadOnly] private List<ITMEventEffect> _bottomEffects = new();

        [SerializeField, ReadOnly] private List<ITMUsage> _topUsages = new();
        [SerializeField, ReadOnly] private List<ITMUsage> _bottomUsages = new();

        [SerializeField, ReadOnly] private UnityEvent<TMEventChoice> _onFireEvent = new();

        public bool CanFireTop => _topUsages.All(usage => usage.CanProcessPayment);
        public bool CanFireBottom => _bottomUsages.All(usage => usage.CanProcessPayment); 

        public void InvokeEvent(TMEventChoice eventChoice)
        {
            if (eventChoice == TMEventChoice.TOP)
            {
                _topEffects.ForEach(effect => effect.ApplyEffect());
                _topUsages.ForEach(usage => usage.ApplyUsage());
            }
            else
            {
                _bottomEffects.ForEach(effect => effect.ApplyEffect());
                _bottomUsages.ForEach(usage => usage.ApplyUsage());
            }

            _onFireEvent.Invoke(eventChoice);
        }
        
        public TMEventRunner(TMEventData eventData)
        {
            EventData = eventData;

            _topEffects = new(EventData.CreateTopEffects());
            _bottomEffects = new(EventData.CreateBottomEffects());

            _topUsages = new(EventData.CreateTopUsages());
            _bottomUsages = new(EventData.CreateBottomUsages());
        }
    }
}