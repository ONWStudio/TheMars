#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Helpers;

namespace OnwAttributeExtensionsEditor
{
    [CustomEditor(typeof(Object), true, isFallback = true)]
    internal sealed class EditorUsingAttributeHelper : Editor
    {
        private readonly List<IObjectEditorAttributeDrawer> _objectEditorAttributeDrawers = new();

        private void OnEnable()
        {
            Debug.Log("EditorUsingHelper Enable");

            _objectEditorAttributeDrawers.AddRange(ReflectionHelper
                .GetChildClassesFromType<IObjectEditorAttributeDrawer>());

            _objectEditorAttributeDrawers
                .ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnEnable(this));
        }

        public override void OnInspectorGUI()
        {
            Debug.Log("EditorUsingHelper OnGUI");

            DrawDefaultInspector();

            _objectEditorAttributeDrawers.
                ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnInspectorGUI(this));
        }
    }
}
#endif