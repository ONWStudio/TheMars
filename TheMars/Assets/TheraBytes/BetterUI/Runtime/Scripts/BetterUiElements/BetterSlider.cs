using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{   
    [HelpURL("https://documentation.therabytes.de/better-ui/BetterSlider.html")]
    [AddComponentMenu("Better UI/Controls/Better Slider", 30)]
    public class BetterSlider : Slider, IBetterTransitionUiElement
    {
        public List<Transitions> BetterTransitions { get { return betterTransitions; } }

        [SerializeField, DefaultTransitionStates]
        private List<Transitions> betterTransitions = new List<Transitions>();

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!(base.gameObject.activeInHierarchy))
                return;

            foreach (Transitions info in betterTransitions)
            {
                info.SetState(state.ToString(), instant);
            }
        }
    }
}
