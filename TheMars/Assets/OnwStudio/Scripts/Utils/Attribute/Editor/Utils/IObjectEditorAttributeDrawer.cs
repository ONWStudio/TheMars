#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Onw.Editor
{
    using Editor = UnityEditor.Editor;

    public interface IObjectEditorAttributeDrawer
    {
        void OnEnable(Editor editor);
        void OnInspectorGUI(Editor editor);
    }
}
#endif
