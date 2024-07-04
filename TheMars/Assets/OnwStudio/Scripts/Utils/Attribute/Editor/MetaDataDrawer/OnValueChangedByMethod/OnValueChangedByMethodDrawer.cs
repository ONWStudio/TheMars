#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using Onw.Helpers;
using static Onw.Editor.EditorHelper;

namespace Onw.Editor
{
    using Editor = UnityEditor.Editor;

    internal sealed class OnValueChangedByMethodDrawer : IObjectEditorAttributeDrawer
    {
        private readonly struct PropertyMethodPair
        {
            public List<Action> Methods { get; }
            public FieldInfo TargetField { get; }
            public object TargetInstance { get; }

            public PropertyMethodPair(List<Action> methods, FieldInfo targetField, object targetInstance)
            {
                Methods = methods;
                TargetField = targetField;
                TargetInstance = targetInstance;
            }
        }

        private readonly Dictionary<string, PropertyMethodPair> _observerMethods = new();
        private readonly List<KeyValuePair<string, string>> _prevProperties = new();

        public void OnEnable(Editor editor)
        {
            if (Application.isPlaying) return;

            _observerMethods.Clear();
            _prevProperties.Clear();

            foreach (Action action in ReflectionHelper.GetActionsFromAttributeAllSearch<OnValueChangedByMethodAttribute>(editor.target))
            {
                OnValueChangedByMethodAttribute onValueChangedByMethodAttribute = action.Method.GetCustomAttribute<OnValueChangedByMethodAttribute>();

                if (!_observerMethods.TryGetValue(onValueChangedByMethodAttribute.FieldName, out PropertyMethodPair methods))
                {
                    FieldInfo targetField = action
                        .Target
                        .GetType()
                        .GetField(onValueChangedByMethodAttribute.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                    if (targetField == null)
                    {
                        Debug.LogWarning("Not found Target Property!");
                        continue;
                    }

                    methods = new(new(), targetField, action.Target);
                    _observerMethods.Add(onValueChangedByMethodAttribute.FieldName, methods);
                }

                methods.Methods.Add(action);
            }

            _prevProperties.AddRange(_observerMethods
                .Select(pair => new KeyValuePair<string, string>(
                    pair.Key,
                    GetPropertyValueFromObject(pair.Value.TargetField.GetValue(pair.Value.TargetInstance))?.ToString() ?? "NULL")));
        }

        public void OnInspectorGUI(Editor editor)
        {
            if (Application.isPlaying) return;

            for (int i = 0; i < _prevProperties.Count; i++)
            {
                string key = _prevProperties[i].Key;
                var propertyMethodPair = _observerMethods[key];
                string nowValue = GetPropertyValueFromObject(
                    propertyMethodPair.TargetField.GetValue(propertyMethodPair.TargetInstance))?.ToString() ?? "NULL";

                if (_prevProperties[i].Value != nowValue)
                {
                    propertyMethodPair
                        .Methods
                        .ForEach(method => method.Invoke());

                    _prevProperties[i] = new(
                         key,
                         GetPropertyValueFromObject(propertyMethodPair.TargetField.GetValue(propertyMethodPair.TargetInstance))?.ToString() ?? "NULL");
                }
            }
        }
    }
}
#endif
