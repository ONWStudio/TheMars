using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649 // disable "never assigned" warnings

namespace TheraBytes.BetterUi
{
    [Serializable]
    public class LocationAnimationTransitions : TransitionStateCollection<string>
    {
        [Serializable]
        public class LocationAnimationTransitionState : TransitionState
        {
            public LocationAnimationTransitionState(string name, string stateObject)
                : base(name, stateObject)
            { }
        }


        public override UnityEngine.Object Target { get { return target; } }

        [SerializeField]
        private LocationAnimations target;

        [SerializeField]
        private List<LocationAnimationTransitionState> states = new List<LocationAnimationTransitionState>();


        public LocationAnimationTransitions(params string[] stateNames)
            : base(stateNames)
        {
        }

        protected override void ApplyState(TransitionState state, bool instant)
        {
            if (this.Target == null)
                return;

            target.StartAnimation(state.StateObject);
        }

        internal override void AddStateObject(string stateName)
        {
            LocationAnimationTransitionState obj = new LocationAnimationTransitionState(stateName, "");
            this.states.Add(obj);
        }

        protected override IEnumerable<TransitionState> GetTransitionStates()
        {
            foreach (LocationAnimationTransitionState s in states)
                yield return s;
        }

        internal override void SortStates(string[] sortedOrder)
        {
            base.SortStatesLogic(states, sortedOrder);
        }
    }
}
