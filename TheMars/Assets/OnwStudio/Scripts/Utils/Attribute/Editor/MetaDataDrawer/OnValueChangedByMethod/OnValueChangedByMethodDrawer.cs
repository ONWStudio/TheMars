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

    /// <summary>
    /// .. 메서드가 특정 필드를 감시합니다
    /// 해당 필드의 값이 변경되면 OnValueChangedByMethod 어트리뷰트를 가진 메서드를 호출합니다
    /// 리플렉션을 통해 값을 찾아내며 비용이 큰 탐색 작업을 수행하므로 값이 자주 변경될 경우 에디터 성능에 이슈가 발생할 수 있습니다
    /// </summary>
    internal sealed class OnValueChangedByMethodDrawer : IObjectEditorAttributeDrawer
    {
        private readonly struct PropertyMethodPair
        {
            public List<Action> Methods { get; } // .. 메서드를 딜리게이트 화 해서 저장합니다 딜리게이트는 인스턴스 정보를 가지고 있으므로 해당 인스턴스 자체의 메서드를 호출할 수 있습니다
            public FieldInfo TargetField { get; } // .. 리플렉션을 할때 필요한 필드 정보를 가지고 있습니다 해당 클래스는 인스턴스 정보는 없으므로 인스턴스는 따로 저장합니다
            public object TargetInstance { get; } // .. 어트리뷰트를 가지고 있는 객체 그 자체가 필요합니다 해당 인스턴스의 메서드를 호출해야하기 때문입니다

            public PropertyMethodPair(List<Action> methods, FieldInfo targetField, object targetInstance)
            {
                Methods = methods;
                TargetField = targetField;
                TargetInstance = targetInstance;
            }
        }

        /// <summary>
        /// .. 타겟으로 하는 오브젝트에서 탐색작업을 수행하므로 어트리뷰트가 여러개 존재할 수 있습니다
        /// 딕셔너리로 필드 이름을 키값으로 하고 같은 필드를 타겟으로 하는 경우가 있을 수 있으므로 메서드를 딜리게이트 화 해서 리스트로 저장합니다
        /// </summary>
        private readonly Dictionary<string, PropertyMethodPair> _observerMethods = new();

        /// <summary>
        /// .. 
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _prevProperties = new();

        public void OnEnable(Editor editor)
        {
            if (Application.isPlaying) return;

            _observerMethods.Clear();
            _prevProperties.Clear();

            // .. 리플렉션 헬퍼를 통해 어떤 인스턴스의 어트리뷰트를 탐색합니다 만약 타겟으로 하는 인스턴스가 계층적으로 타겟으로 하는 어트리뷰트를 보유한 인스턴스를 List나 필드의 형태로 보유하는 모든 경우를 탐색합니다
            // 내부적으로 ReflectionHelper.GetActionsFromAttributeAllSearch<OnValueChangedByMethodAttribute>() 메서드를 계속해서 호출하기 때문에
            // 구조적으로 클래스간 상호 참조가 일어나는 경우 스택 오버 플로우가 발생할 수 있습니다
            // 클래스간 상호 참조에 의한 함수의 무한 호출을 방지하기 위해서 내부적으로 HashSet을 통해 중복 클래스 검사를 방지하고 있습니다
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
                    pair.Key, // .. 키값 저장 _observerMethod의 키값으로 사용됩니다
                    GetPropertyValueFromObject(pair.Value.TargetField.GetValue(pair.Value.TargetInstance))?.ToString() ?? "NULL"))); // .. 필드의 현재 값을 캐시합니다 
        }

        public void OnInspectorGUI(Editor editor)
        {
            if (Application.isPlaying) return;

            for (int i = 0; i < _prevProperties.Count; i++)
            {
                string key = _prevProperties[i].Key; // .. 메서드 리스트의 키값
                var propertyMethodPair = _observerMethods[key];
                string nowValue = GetPropertyValueFromObject( // .. 현재 키값에 해당하는 필드의 값을 불러옵니다
                    propertyMethodPair.TargetField.GetValue(propertyMethodPair.TargetInstance))?.ToString() ?? "NULL";

                Debug.Log(nowValue);

                if (_prevProperties[i].Value != nowValue) // .. 만약 값이 변했다면?
                {
                    propertyMethodPair // .. 해당 Field를 타겟으로 하는 모든 메서드 호출
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
