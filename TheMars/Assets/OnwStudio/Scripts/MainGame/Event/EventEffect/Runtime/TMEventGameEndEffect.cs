using Onw.Attribute;
using System.Collections;
using System.Collections.Generic;
using TM.Event.Effect;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

namespace TM.Event
{
    [System.Serializable]
    public sealed class TMEventGameEndEffect : ITMEventEffect
    {
        [field: SerializeField, ReadOnly] public LocalizedString EffectDescription { get; private set; } = new("TM_Event_Effect", "Game_End_Effect");

        public void ApplyEffect()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}