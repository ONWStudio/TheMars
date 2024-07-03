#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Helpers;

namespace OnwAttributeExtensionsEditor
{
    [CustomEditor(typeof(Object), true)]
    internal sealed class EditorUsingAttributeHelper : Editor
    {
        private readonly List<IObjectEditorAttributeDrawer> _objectEditorAttributeDrawers = new();
        private Editor _defaultEditor =  null;

        private void OnEnable()
        {
            Debug.Log("EditorUsingHelper Enable");
            _defaultEditor = CreateEditor(target, EditorUtility.InstanceIDToObject(target.GetInstanceID()).GetType());

            _objectEditorAttributeDrawers.AddRange(ReflectionHelper
                .GetChildClassesFromType<IObjectEditorAttributeDrawer>());
            _objectEditorAttributeDrawers
                .ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnEnable(this));
        }

        public override void OnInspectorGUI()
        {
            Debug.Log("EditorUsingHelper OnGUI");
            // 기본 인스펙터 GUI를 그립니다.
            if (_defaultEditor)
            {
                _defaultEditor.OnInspectorGUI();
                Debug.Log("Default");
            }

            _objectEditorAttributeDrawers.
                ForEach(objectEditorAttributeDrawer => objectEditorAttributeDrawer.OnInspectorGUI(this));
        }
    }
}
#endif