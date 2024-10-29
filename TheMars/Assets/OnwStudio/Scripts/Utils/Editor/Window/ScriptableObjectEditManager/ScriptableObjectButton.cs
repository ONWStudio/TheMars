#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;

namespace Onw.Editor.Window
{
    internal sealed class ScriptableObjectButton : Button
    {
        public ScriptableObject ScriptableObject { get; private set; } = null;

        public ScriptableObjectButton(ScriptableObject scriptableObject, Action clickedEvent) : base(clickedEvent) 
        {
            ScriptableObject = scriptableObject;
        }

        public ScriptableObjectButton(ScriptableObject scriptableObject) : base() 
        {
            ScriptableObject = scriptableObject;
        }
    }
}
#endif
