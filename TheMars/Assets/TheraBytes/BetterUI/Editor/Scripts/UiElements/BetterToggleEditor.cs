using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi.Editor
{
    [CustomEditor(typeof(BetterToggle)), CanEditMultipleObjects]
    public class BetterToggleEditor : ToggleEditor
    {
        private BetterElementHelper<Toggle, BetterToggle> transitions = 
            new BetterElementHelper<Toggle, BetterToggle>();

        private BetterElementHelper<Toggle, BetterToggle> transitionsWhenOn =
            new BetterElementHelper<Toggle, BetterToggle>("betterTransitionsWhenOn");

        private BetterElementHelper<Toggle, BetterToggle> transitionsWhenOff =
            new BetterElementHelper<Toggle, BetterToggle>("betterTransitionsWhenOff");

        private BetterElementHelper<Toggle, BetterToggle> OnOffTransitions =
            new BetterElementHelper<Toggle, BetterToggle>("betterToggleTransitions");


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BetterToggle tgl = target as BetterToggle;
            transitions.DrawGui(serializedObject);
            OnOffTransitions.DrawGui(serializedObject);
            transitionsWhenOn.DrawGui(serializedObject);
            transitionsWhenOff.DrawGui(serializedObject);

            serializedObject.ApplyModifiedProperties();
        }
        
        [MenuItem("CONTEXT/Toggle/♠ Make Better")]
        public static void MakeBetter(MenuCommand command)
        {
            Toggle tgl = command.context as Toggle;
            Betterizer.MakeBetter<Toggle, BetterToggle>(tgl);
        }
    }
}
