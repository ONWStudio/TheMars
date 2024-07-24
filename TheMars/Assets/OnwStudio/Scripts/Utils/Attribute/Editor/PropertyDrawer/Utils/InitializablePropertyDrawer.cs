#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Onw.Attribute.Editor
{
    internal abstract class InitializablePropertyDrawer : PropertyDrawer
    {
        private bool _isInitialized = false;

        protected abstract void OnEnable(Rect position, SerializedProperty property, GUIContent label);
        protected abstract void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label);

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                OnEnable(position, property, label);
            }

            OnPropertyGUI(position, property, label);
        }
    }
}
#endif
