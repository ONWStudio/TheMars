#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TheMarsGUITool
{
    internal sealed partial class TheMarsGUIToolDrawer : EditorWindow
    {
        internal static class DataHandler<T> where T : ScriptableObject
        {
            internal static T CreateScriptableObject(string dataPath)
            {
                return CreateInstance<T>();
            }
        }
    }
}
#endif
