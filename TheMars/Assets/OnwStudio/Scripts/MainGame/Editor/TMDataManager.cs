#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Onw.Helper;
using Onw.Editor.Window;
using Onw.ScriptableObjects.Editor;
using TM.Card;
using TM.Event;
using TM.Synergy;
using TM.Building;

namespace TM.Editor
{
    internal sealed class TMDataManager : ScriptableObjectViewer
    {
        [MenuItem("Onw Studio/TM/Data")]
        private static void showWindow()
        {
            CreateWindow<TMDataManager>().Show();
        }

        protected override ScrollBuildOption[] GetTargetScriptableObjectType()
        {
            ScrollBuildOption[] types =
            {
                new(typeof(TMCardData), typeof(TMCardScrollView), "카드"),
                new(typeof(TMBuildingData), typeof(TMBuildingScrollView),"건물"),
                new(typeof(TMSynergyData), typeof(TMSynergyScrollView), "시너지"),
                new(typeof(TMEventData), typeof(TMEventScrollView), "이벤트")
            };

            return types;
        }
    }
}
#endif