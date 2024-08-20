#if UNITY_EDITOR
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Editor;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(OnChangedValueByValueAttribute))]
    internal sealed class OnChangedValueByValueDrawer : InitializablePropertyDrawer
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private OnChangedValueByValueAttribute _attribute;
        private MethodInfo _methodInfo;
        private object _target;

        protected override void OnEnable(Rect position, SerializedProperty property, GUIContent label)
        {
            _attribute = attribute as OnChangedValueByValueAttribute;
            _target = property.serializedObject.targetObject;
            _methodInfo = _target.GetType().GetMethod(_attribute.MethodName, FLAGS);
        }

        protected override void OnPropertyGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying) return;

            using EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope();
            EditorGUI.PropertyField(position, property, label, true);

            if (scope.changed)
            {
                if (_methodInfo is not null)
                {
                    _methodInfo.Invoke(_target, null);
                }
                else
                {
                    Debug.LogWarning($"Method {_attribute.MethodName} not found on {_target.GetType().Name}");
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
