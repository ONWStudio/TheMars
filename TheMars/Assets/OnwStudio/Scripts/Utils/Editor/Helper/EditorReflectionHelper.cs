#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

namespace Onw.Editor
{
    using Attribute = System.Attribute;

    public static class EditorReflectionHelper
    {
        public static FieldInfo GetFieldFromMethod(MethodInfo method, string fieldName)
        {
            if (method == null || string.IsNullOrEmpty(fieldName)) return null;

            // 메서드가 속한 클래스의 타입을 가져옴
            Type targetType = method.DeclaringType;

            // 필드 이름으로 필드를 검색
            FieldInfo fieldInfo = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fieldInfo;
        }

        public static IEnumerable<CastType> GetSubclassesOfGenericClass<CastType>(Type genericType) where CastType : class
        {
            if (!genericType.IsGenericTypeDefinition)
            {
                Debug.LogWarning("Is Not GenericType");
                yield break;
            }

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (type.BaseType is null || 
                    !type.BaseType.IsGenericType || 
                    type.BaseType.GetGenericTypeDefinition() != genericType.GetGenericTypeDefinition()) continue;

                foreach (CastType castObject in Resources.FindObjectsOfTypeAll(type).OfType<CastType>())
                {
                    yield return castObject;
                }
            }
        }

        public static IEnumerable<Type> GetSubclassTypeFromGenericType(Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
            {
                Debug.LogWarning("Is Not GenericType");
                yield break;
            }

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (type.BaseType is null ||
                    !type.BaseType.IsGenericType ||
                    type.BaseType.GetGenericTypeDefinition() != genericType.GetGenericTypeDefinition()) continue;

                yield return type;
            }
        }
        /// <summary>
        /// .. 자동구현 프로퍼티의 백킹 필드의 직렬화 된 이름을 가져옵니다.
        /// </summary>
        /// 프로퍼티 이름
        /// </param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetAttributes(object targetField)
        {
            if (targetField is null) yield break;

            foreach (Attribute attribute in targetField.GetType().GetCustomAttributes<Attribute>())
            {
                yield return attribute;
            }
        }

        public static IEnumerable<MethodInfo> GetMethodsFromAttribute<AttributeType>(object @object) where AttributeType : class
        {
            Type currentType = @object.GetType();

            while (isSearch(currentType))
            {
                foreach (MethodInfo methodInfo in currentType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methodInfo.GetCustomAttribute(typeof(AttributeType)) is not AttributeType) continue;

                    yield return methodInfo;
                }

                currentType = currentType.BaseType;
            }

            static bool isSearch(Type currentType) => currentType != null &&
                currentType != typeof(MonoBehaviour) &&
                currentType != typeof(Component) &&
                currentType != typeof(ScriptableObject) &&
                currentType != typeof(GameObject);
        }

        /// <summary>
        /// .. 해당 메서드는 검사기에 표시가능한 클래스나 인터페이스에서 어떤 Attribute를 소유한 메서드를 딜리게이트화(인스턴스 정보를 가지고 있는 메서드 정보)를 해서 찾아옵니다
        /// 클래스 인스턴스에서 MonoBehaviour, Component, ScriptableObject를 제외하고 참조가능한 클래스 인스턴스가 있을때 해당 인스턴스를 모두 탐색하여 Attirbute를 찾아옵니다
        /// 만약 클래스간 상호참조가 있을 경우는 이미 검사한 인스턴스는 제외함으로써 무한 순환 참조를 방지합니다
        /// SerializedReference로 어떤 인스턴스를 참조중인 경우에 사용할 수 있습니다
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="target"></param>
        /// <param name="visited"></param>
        /// <returns></returns>
        public static IEnumerable<Action> GetActionsFromAttributeAllSearch<AttributeType>(object target, HashSet<object> visited = null) where AttributeType : class
        {
            visited ??= new HashSet<object>();

            if (target == null || visited.Contains(target)) // .. 해쉬셋으로 중복검사 방지
            {
                yield break;
            }

            visited.Add(target);
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            Type type = target.GetType();
            Type monoType = typeof(MonoBehaviour);
            Type componentType = typeof(Component);
            Type scriptableObjectType = typeof(ScriptableObject);
            Type obsoleteAttributeType = typeof(ObsoleteAttribute);

            // .. target에 대한 attribute가 적용된 메서드 찾아내기
            foreach (MethodInfo method in GetMethodsFromAttribute<AttributeType>(target))
            {
                yield return method.IsStatic ? (Action)Delegate.CreateDelegate(typeof(Action), method) :
                    (Action)Delegate.CreateDelegate(typeof(Action), target, method);
            }

            // .. target이 계층적으로 클래스를 보유중이라면 target이 보유한 클래스들까지 검사
            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                if (checkIgnoreInfo(field, obsoleteAttributeType) ||
                    checkIgnoreType(field.FieldType, monoType, componentType, scriptableObjectType)) continue;

                foreach (Action action in getActionEnumerableFromInfo(field.GetValue(target), visited))
                {
                    yield return action;
                }
            }

            // .. 마찬가지로 자동구현 프로퍼티에 의한 참조 클래스들도 검사
            foreach (PropertyInfo property in type.GetProperties(bindingFlags))
            {
                if (!property.CanRead ||
                    property.GetMethod is null ||
                    (!property.GetMethod.IsStatic && target is null) ||
                    checkIgnoreInfo(property, obsoleteAttributeType) ||
                    checkIgnoreType(property.PropertyType, monoType, componentType, scriptableObjectType)) continue;

                foreach (Action action in getActionEnumerableFromInfo(property.GetValue(target), visited))
                {
                    yield return action;
                }
            }

            static IEnumerable<Action> getActionEnumerableFromInfo(object value, HashSet<object> visited)
            {
                if (value == null) yield break;

                // .. 필드라면
                foreach (Action action in GetActionsFromAttributeAllSearch<AttributeType>(value, visited))
                {
                    yield return action;
                }

                // .. 만약 열거된 데이터 자료구조라면?
                if (value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is null) continue;

                        foreach (Action action in GetActionsFromAttributeAllSearch<AttributeType>(item, visited))
                        {
                            yield return action;
                        }
                    }
                }
            }
        }

        // .. Count를 가지고 있는 
        public static IEnumerable<ICollection> GetCollectionsFromSerializedField(object target, HashSet<object> visited = null)
        {
            visited ??= new();

            if (target == null || visited.Contains(target)) // .. 해쉬셋으로 중복검사 방지
            {
                yield break;
            }

            visited.Add(target);

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            Type type = target.GetType();
            Type monoType = typeof(MonoBehaviour);
            Type componentType = typeof(Component);
            Type scriptableObjectType = typeof(ScriptableObject);
            Type obsoleteAttributeType = typeof(ObsoleteAttribute);

            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                if (checkIgnoreInfo(field, obsoleteAttributeType) ||
                    checkIgnoreType(field.FieldType, monoType, componentType, scriptableObjectType)) continue;

                foreach (var collection in getCollectionsFromValue(field.GetValue(target), visited))
                {
                    yield return collection;
                }
            }

            foreach (PropertyInfo property in type.GetProperties(bindingFlags))
            {
                if (!property.CanRead ||
                    property.GetMethod is null ||
                    (!property.GetMethod.IsStatic && target is null) ||
                    checkIgnoreInfo(property, obsoleteAttributeType) ||
                    checkIgnoreType(property.PropertyType, monoType, componentType, scriptableObjectType)) continue;

                foreach (var collection in getCollectionsFromValue(property.GetValue(target), visited))
                {
                    yield return collection;
                }
            }

            static IEnumerable<ICollection> getCollectionsFromValue(object value, HashSet<object> visited)
            {
                if (value == null) yield break;

                if (value is ICollection collection)
                {
                    yield return collection;
                }

                foreach (var subCollection in GetCollectionsFromSerializedField(value, visited))
                {
                    yield return subCollection;
                }

                if (value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is null) continue;

                        foreach (var subCollection in GetCollectionsFromSerializedField(item, visited))
                        {
                            yield return subCollection;
                        }
                    }
                }
            }
        }

        public static IEnumerable<Attribute> GetAttributes(object targetObject, string propertyPath)
        {
            if (targetObject is null) yield break;

            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            if (field is not null)
            {
                foreach (Attribute attribute in field.GetCustomAttributes<Attribute>())
                {
                    yield return attribute;
                }
            }
        }

        public static FieldInfo GetFieldInfo(object targetObject, string propertyPath)
        {
            if (targetObject is null || string.IsNullOrEmpty(propertyPath)) return null;

            return targetObject
                .GetType()
                .GetField(propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        public static Type GetPropertyType(object targetObject, string propertyPath)
        {
            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(UnityEngine.Object))
            {
                var genericArgs = field.FieldType.GetGenericArguments();
                if (genericArgs.Length > 0)
                {
                    return genericArgs[0];
                }
            }

            return field.FieldType;
        }

        public static bool IsNestedAttribute<AttributeType>(object targetObject) where AttributeType : Attribute
        {
            return targetObject is not null && targetObject.GetType().GetCustomAttribute<AttributeType>() is not null;
        }

        public static bool IsNestedAttribute<AttributeType>(object targetObject, string propertyPath) where AttributeType : Attribute
        {
            FieldInfo field = GetFieldInfo(targetObject, propertyPath);

            return field is not null && field.GetCustomAttribute<AttributeType>() is not null;
        }

        public static string GetBackingFieldName(string propertyName)
            => $"<{propertyName}>k__BackingField";

        private static bool checkIgnoreType(Type fieldType, Type monoType, Type componentType, Type scriptableObjectType)
        {
            return (!fieldType.IsClass && !fieldType.IsInterface) ||
                monoType.IsAssignableFrom(fieldType) ||
                componentType.IsAssignableFrom(fieldType) ||
                scriptableObjectType.IsAssignableFrom(fieldType);
        }

        private static bool checkIgnoreInfo(MemberInfo memInfo, Type obsoleteAttributeType)
        {
            return memInfo.IsDefined(obsoleteAttributeType, true) ||
                (memInfo.GetCustomAttribute<SerializeReference>() is null && memInfo.GetCustomAttribute<SerializeField>() is null);
        }
    }
}
#endif