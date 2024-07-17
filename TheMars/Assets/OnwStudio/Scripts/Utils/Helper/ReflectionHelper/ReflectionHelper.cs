using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<MethodInfo> GetMethodsFromAttribute<AttributeType>(object @object) where AttributeType : class
            => @object
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttribute(typeof(AttributeType)) is AttributeType);

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
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

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

            static bool checkIgnoreType(Type fieldType, Type monoType, Type componentType, Type scriptableObjectType)
            {
                return (!fieldType.IsClass && !fieldType.IsInterface) ||
                    monoType.IsAssignableFrom(fieldType) ||
                    componentType.IsAssignableFrom(fieldType) ||
                    scriptableObjectType.IsAssignableFrom(fieldType);
            }

            static bool checkIgnoreInfo(MemberInfo memInfo, Type obsoleteAttributeType)
            {
                return memInfo.IsDefined(obsoleteAttributeType, true) ||
                    (memInfo.GetCustomAttribute<SerializeReference>() is null && memInfo.GetCustomAttribute<SerializeField>() is null);
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

        public static FieldInfo GetFieldFromMethod(MethodInfo method, string fieldName)
        {
            if (method == null || string.IsNullOrEmpty(fieldName)) return null;

            // 메서드가 속한 클래스의 타입을 가져옴
            Type targetType = method.DeclaringType;

            // 필드 이름으로 필드를 검색
            FieldInfo fieldInfo = targetType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fieldInfo;
        }

        public static IEnumerable<T> GetChildClassesFromType<T>() where T : class
        {
            Type baseType = typeof(T);

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (type.IsInterface || type.IsAbstract || !baseType.IsAssignableFrom(type)) continue;

                yield return Activator.CreateInstance(type) as T;
            }
        }

        public static IEnumerable<string> GetClassNamesFromParent(string baseClass)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type baseType = assembly.GetType(baseClass);

                if (baseType is not null)
                {
                    var names = assembly
                        .GetTypes()
                        .Where(type => type.IsSubclassOf(baseType) || type.GetInterfaces().Contains(type))
                        .Select(type => type.Name);

                    foreach (string name in names)
                    {
                        yield return name;
                    }
                }
            }
        }

        public static IEnumerable<string> GetClassNamesFromParent<BaseType>() where BaseType : class
        {
            Type type = typeof(BaseType);

            foreach (Type someType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (someType.IsAbstract ||
                    someType.IsInterface ||
                    (!someType.IsSubclassOf(type) && !someType.GetInterfaces().Contains(type))) continue;

                yield return someType.Name;
            }
        }

        public static IEnumerable<Type> GetChildClassesFromBaseType(Type baseType)
            => AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        public static IEnumerable<Type> GetChildClassesFromFieldTypeName(string typeName)
        {
            Type baseType = GetTypeFromFieldName(typeName);

            if (baseType is null)
            {
                Debug.LogWarning("Not Found BaseType!");
                return null;
            }

            return GetChildClassesFromBaseType(baseType);
        }

        public static Type GetTypeFromFieldName(string typeName)
        {
            string[] splitTypeNames = typeName.Split(' ', '.');

            if (splitTypeNames.Length <= 0)
            {
                Debug.LogWarning("typeName is Empty");
                return null;
            }

            string assemblyName = splitTypeNames[0];
            string baseTypeName = splitTypeNames[^1];

            Assembly targetAssembly = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);

            if (targetAssembly is null)
            {
                Debug.LogWarning("Not Found Assembly!");
                return null;
            }

            Type baseType = targetAssembly
                .GetTypes()
                .SingleOrDefault(type => type.Name == baseTypeName);

            if (baseType is null)
            {
                Debug.LogWarning("Not Found BaseType!");
            }

            return baseType;
        }

        public static IEnumerable<string> GetEnumValuesFromEnumName(string enumTypeName)
        {
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (!type.IsEnum || type.Name != enumTypeName) continue;

                foreach (string name in Enum.GetNames(type))
                {
                    yield return name;
                }
            }
        }

        public static Dictionary<string, int> GetEnumKVPFromEnumName(string enumTypeName)
        {
            Dictionary<string, int> enumValues = new();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var enumType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.IsEnum && t.Name == enumTypeName);

                if (enumType != null)
                {
                    var underlyingType = Enum.GetUnderlyingType(enumType);

                    foreach (var name in Enum.GetNames(enumType))
                    {
                        var enumValue = Enum.Parse(enumType, name);
                        var value = Convert.ToInt32(Convert.ChangeType(enumValue, underlyingType));
                        enumValues.Add(name, value);
                    }

                    break;
                }
            }

            return enumValues;
        }
    }
}
