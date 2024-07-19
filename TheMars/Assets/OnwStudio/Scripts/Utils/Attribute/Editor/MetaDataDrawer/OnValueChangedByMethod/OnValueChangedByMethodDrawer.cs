#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
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
        private readonly List<ICollection> _collections = new();
        private readonly List<int> _prevCollectionCounts = new();
        private readonly List<string> _prevCollectionStates = new();

        public void OnEnable(Editor editor)
        {
            Initialize(editor);
        }

        private void Initialize(Editor editor)
        {
            if (Application.isPlaying) return;

            _observerMethods.Clear();
            _prevProperties.Clear();
            _collections.Clear();
            _prevCollectionCounts.Clear();
            _prevCollectionStates.Clear();

            _collections.AddRange(EditorReflectionHelper.GetCollectionsFromSerializedField(editor.target));
            _prevCollectionCounts.AddRange(_collections.Select(collection => collection.Count));
            _prevCollectionStates.AddRange(_collections.Select(collection => computeCollectionState(collection)));

            foreach (Action action in EditorReflectionHelper.GetActionsFromAttributeAllSearch<OnValueChangedByMethodAttribute>(editor.target))
            {
                OnValueChangedByMethodAttribute onValueChangedByMethodAttribute = action.Method.GetCustomAttribute<OnValueChangedByMethodAttribute>();

                foreach (string fieldName in onValueChangedByMethodAttribute.FieldName)
                {
                    if (!_observerMethods.TryGetValue(fieldName, out PropertyMethodPair methods))
                    {
                        FieldInfo targetField = getFieldInfo(action, fieldName) ?? getFieldInfo(action, EditorReflectionHelper.GetBackingFieldName(fieldName));

                        if (targetField == null)
                        {
                            Debug.LogWarning("Not found Target Property!");
                            continue;
                        }

                        methods = new PropertyMethodPair(new List<Action>(), targetField, action.Target);
                        _observerMethods.Add(fieldName, methods);
                    }

                    methods.Methods.Add(action);
                }
            }

            _prevProperties.AddRange(_observerMethods
                .Select(pair => new KeyValuePair<string, string>(
                    pair.Key,
                    GetPropertyValueFromObject(pair.Value.TargetField.GetValue(pair.Value.TargetInstance))?.ToString() ?? "NULL")));

            static FieldInfo getFieldInfo(Action action, string fieldName)
            {
                return action
                    .Target
                    .GetType()
                    .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            }
        }

        public void OnInspectorGUI(Editor editor)
        {
            if (Application.isPlaying) return;

            bool isUpdate = false;
            for (int i = 0; i < _collections.Count; i++)
            {
                if (_collections[i].Count != _prevCollectionCounts[i] || computeCollectionState(_collections[i]) != _prevCollectionStates[i])
                {
                    isUpdate = true;
                    break;
                }
            }

            for (int i = 0; i < _prevProperties.Count; i++)
            {
                string key = _prevProperties[i].Key;
                var propertyMethodPair = _observerMethods[key];
                string nowValue = GetPropertyValueFromObject(
                    propertyMethodPair.TargetField.GetValue(propertyMethodPair.TargetInstance))?.ToString() ?? "NULL";

                if (_prevProperties[i].Value != nowValue)
                {
                    propertyMethodPair.Methods.ForEach(method => EditorApplication.delayCall += () =>
                    {
                        method.Invoke();
                        EditorUtility.SetDirty(editor.target);
                    });

                    _prevProperties[i] = new KeyValuePair<string, string>(
                        key,
                        GetPropertyValueFromObject(propertyMethodPair.TargetField.GetValue(propertyMethodPair.TargetInstance))?.ToString() ?? "NULL");
                }
            }

            if (isUpdate)
            {
                Initialize(editor);
            }
        }

        private string computeCollectionState(ICollection collection)
        {
            var elementStates = collection.Cast<object>().Select(e => e?.GetHashCode().ToString() ?? "null");
            return string.Join(",", elementStates);
        }
    }
}
#endif