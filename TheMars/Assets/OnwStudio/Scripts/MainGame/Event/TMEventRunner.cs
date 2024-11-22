using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.Attribute;
using TM.Event.Effect;
using TM.Cost;

namespace TM.Event
{
    public interface ITMEventRunner
    {
        event UnityAction<TMEventChoice> OnFireEvent;
        ITMEventData EventReadData { get; }

        IReadOnlyList<ITMEventEffect> TopEffects { get; }
        IReadOnlyList<ITMEventEffect> BottomEffects { get; }

        IReadOnlyList<ITMCost> TopCosts { get; }
        IReadOnlyList<ITMCost> BottomCosts { get; }

        bool CanFireTop { get; }
        bool CanFireBottom { get; }

        void InvokeEvent(TMEventChoice eventChoice);
    }

    [System.Serializable]
    public sealed class TMEventRunner : ITMEventRunner
    {
        public event UnityAction<TMEventChoice> OnFireEvent
        {
            add => _onFireEvent.AddListener(value);
            remove => _onFireEvent.RemoveListener(value);
        }

        public ITMEventData EventReadData => EventData;
        public IReadOnlyList<ITMEventEffect> TopEffects => _topEffects;
        public IReadOnlyList<ITMEventEffect> BottomEffects => _bottomEffects;

        public IReadOnlyList<ITMCost> TopCosts => _topCosts;
        public IReadOnlyList<ITMCost> BottomCosts => _bottomCosts;

        [field: SerializeField] public TMEventData EventData { get; private set; }

        [SerializeReference, ReadOnly] private List<ITMEventEffect> _topEffects = new();
        [SerializeReference, ReadOnly] private List<ITMEventEffect> _bottomEffects = new();

        [SerializeReference, ReadOnly] private List<ITMCost> _topCosts = new();
        [SerializeReference, ReadOnly] private List<ITMCost> _bottomCosts = new();

        [SerializeField, ReadOnly] private UnityEvent<TMEventChoice> _onFireEvent = new();

        public bool CanFireTop => _topCosts.All(usage => usage.CanProcessPayment);
        public bool CanFireBottom => _bottomCosts.All(usage => usage.CanProcessPayment); 

        public void InvokeEvent(TMEventChoice eventChoice)
        {
            if (eventChoice == TMEventChoice.TOP)
            {
                _topEffects.ForEach(effect => effect.ApplyEffect());
                _topCosts.ForEach(usage => usage.ApplyCosts());
            }
            else
            {
                _bottomEffects.ForEach(effect => effect.ApplyEffect());
                _bottomCosts.ForEach(usage => usage.ApplyCosts());
            }

            _onFireEvent.Invoke(eventChoice);
        }
        
        public TMEventRunner(TMEventData eventData)
        {
            EventData = eventData;

            _topEffects = new(EventData.CreateTopEffects());
            _bottomEffects = new(EventData.CreateBottomEffects());

            _topCosts = new(EventData.CreateTopCosts());
            _bottomCosts = new(EventData.CreateBottomCosts());
        }
    }
}