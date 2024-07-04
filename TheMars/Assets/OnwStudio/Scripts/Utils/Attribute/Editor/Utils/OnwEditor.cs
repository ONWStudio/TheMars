#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Helpers;

namespace Onw.Editor
{
    using Editor = UnityEditor.Editor;

    //[CustomEditor(typeof(Object), true)]
    //public class OnwEditor : Editor
    //{
    //    private readonly List<IObjectEditorAttributeDrawer> _objectEditorAttributeDrawers = new();

    //    protected virtual void OnEnable()
    //    {
    //        Debug.Log("EditorUsingHelper Enable");
    //        _objectEditorAttributeDrawers.AddRange(ReflectionHelper
    //            .GetChildClassesFromType<IObjectEditorAttributeDrawer>());
    //        _objectEditorAttributeDrawers
    //            .ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnEnable(this));
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        Debug.Log("EditorUsingHelper OnGUI");
    //        DrawDefaultInspector();

    //        _objectEditorAttributeDrawers.
    //            ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnInspectorGUI(this));
    //    }
    //}
}
#endif