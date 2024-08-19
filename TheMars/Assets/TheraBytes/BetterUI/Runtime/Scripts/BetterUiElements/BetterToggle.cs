using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{
    [HelpURL("https://documentation.therabytes.de/better-ui/BetterToggle.html")]
    [AddComponentMenu("Better UI/Controls/Better Toggle", 30)]
    public class BetterToggle : Toggle, IBetterTransitionUiElement
    {
        public List<Transitions> BetterTransitions { get { return betterTransitions; } }
        public List<Transitions> BetterTransitionsWhenOn { get { return betterTransitionsWhenOn; } }
        public List<Transitions> BetterTransitionsWhenOff { get { return betterTransitionsWhenOff; } }
        public List<Transitions> BetterToggleTransitions { get { return betterToggleTransitions; } }

        [SerializeField, DefaultTransitionStates]
        private List<Transitions> betterTransitions = new List<Transitions>();

        [SerializeField, TransitionStates("On", "Off")]
        private List<Transitions> betterToggleTransitions = new List<Transitions>();
        [SerializeField, DefaultTransitionStates]
        private List<Transitions> betterTransitionsWhenOn = new List<Transitions>();
        [SerializeField, DefaultTransitionStates]
        private List<Transitions> betterTransitionsWhenOff = new List<Transitions>();

        private bool wasOn;

        protected override void OnEnable()
        {
            base.OnEnable();
            ValueChanged(base.isOn, true);
            DoStateTransition(SelectionState.Normal, true);
        }

        private void Update()
        {
            if (wasOn != isOn)
            {
                ValueChanged(isOn);
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!(base.gameObject.activeInHierarchy))
                return;

            List<Transitions> stateTransitions = (isOn)
                ? betterTransitionsWhenOn
                : betterTransitionsWhenOff;

            foreach (Transitions info in stateTransitions)
            {
                info.SetState(state.ToString(), instant);
            }

            foreach (Transitions info in betterTransitions)
            {
                if (state != SelectionState.Disabled && isOn)
                {
                    Transitions tglTr = betterToggleTransitions.FirstOrDefault(
                        (o) => o.TransitionStates != null && info.TransitionStates != null
                            && o.TransitionStates.Target == info.TransitionStates.Target
                            && o.Mode == info.Mode);

                    if (tglTr != null)
                    {
                        continue;
                    }
                }

                info.SetState(state.ToString(), instant);
            }
        }

        private void ValueChanged(bool on)
        {
            ValueChanged(on, false);
        }

        private void ValueChanged(bool on, bool immediate)
        {
            wasOn = on;
            foreach (Transitions state in betterToggleTransitions)
            {
                state.SetState((on) ? "On" : "Off", immediate);
            }

            List<Transitions> stateTransitions = (on)
                ? betterTransitionsWhenOn
                : betterTransitionsWhenOff;

            foreach (Transitions state in stateTransitions)
            {
                state.SetState(currentSelectionState.ToString(), immediate);
            }
        }

    }
}
