#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorTool.EditorTool;
using OnwAttributeExtensions;

namespace OnwAttributeExtensionsEditor
{
    internal sealed class OnValueChangedByMethodDrawer : IObjectEditorAttributeDrawer
    {
        private readonly struct PropertyMethodPair
        {
            public List<MethodInfo> Methods { get; }
            public SerializedProperty TargetProperty { get; }

            public PropertyMethodPair(List<MethodInfo> methods, SerializedProperty targetProperty)
            {
                Methods = methods;
                TargetProperty = targetProperty;
            }
        }

        private readonly Dictionary<string, PropertyMethodPair> _observerMethods = new();
        private readonly List<KeyValuePair<string, SerializedProperty>> _prevProperties = new();

        public void OnEnable(Editor editor)
        {
            if (Application.isPlaying) return;

            _observerMethods.Clear();
            _prevProperties.Clear();

            foreach (MethodInfo methodInfo in ReflectionHelper.GetMethodsFromAttribute<OnValueChangedByMethodAttribute>(editor.target))
            {
                OnValueChangedByMethodAttribute onValueChangedByMethodAttribute = methodInfo.GetCustomAttribute<OnValueChangedByMethodAttribute>();

                if (!_observerMethods.TryGetValue(onValueChangedByMethodAttribute.FieldName, out PropertyMethodPair methods))
                {
                    SerializedProperty targetProperty = GetProperty(editor.serializedObject, onValueChangedByMethodAttribute.FieldName);

                    if (targetProperty is null)
                    {
                        Debug.LogWarning("Not found Target Property!");
                    }

                    methods = new(new(), targetProperty);
                    _observerMethods.Add(onValueChangedByMethodAttribute.FieldName, methods);
                }

                methods.Methods.Add(methodInfo);
            }

            _prevProperties.AddRange(_observerMethods
                .Values
                .Select(pair => new KeyValuePair<string, SerializedProperty>(GetPropertyValue(pair.TargetProperty).ToString(), pair.TargetProperty)));
        }

        public void OnInspectorGUI(Editor editor)
        {
            if (Application.isPlaying) return;

            for (int i = 0; i < _prevProperties.Count; i++)
            {
                string key = _prevProperties[i].Value.displayName.Replace(" ", "");
                string nowValue = GetPropertyValue(_observerMethods[key].TargetProperty).ToString();

                if (_prevProperties[i].Key != nowValue)
                {
                    _observerMethods[key]
                        .Methods
                        .ForEach(method => method.Invoke(editor.target, null));

                    _prevProperties[i] = new(
                        GetPropertyValue(_observerMethods[key].TargetProperty).ToString(),
                        _observerMethods[key].TargetProperty);
                }
            }
        }
    }
}
#endif
