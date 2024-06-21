#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OnwAttributeExtensionsEditor
{
    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    internal sealed class EditorUsingAttributeHelper : Editor
    {
        private readonly List<IObjectEditorAttributeDrawer> _objectEditorAttributeDrawers = new();

        private void OnEnable()
        {
            _objectEditorAttributeDrawers.AddRange(ReflectionHelper
                .GetChildClassesFromType<IObjectEditorAttributeDrawer>());

            _objectEditorAttributeDrawers
                .ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnEnable(this));
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _objectEditorAttributeDrawers.
                ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnInspectorGUI(this));
        }
    }
}
#endif