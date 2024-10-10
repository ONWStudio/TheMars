#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Onw.Editor
{
    public abstract class InitializablePropertyDrawer : PropertyDrawer
    {
        private static readonly HashSet<int> _hashSet = new();
        protected abstract void OnEnable(Rect position, SerializedProperty property, GUIContent label);
        protected abstract void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label);

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_hashSet.Contains(property.GetHashCode()))
            {
                OnEnable(position, property, label);
                _hashSet.Add(property.GetHashCode());
            }

            OnPropertyGUI(position, property, label);
        }
    }
}
#endif
