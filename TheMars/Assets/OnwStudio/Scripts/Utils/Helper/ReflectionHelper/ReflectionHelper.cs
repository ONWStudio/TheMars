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
        public static IEnumerable<T> CreateChildClassesFromType<T>() where T : class
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
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsSubclassOf(baseType) || !type.GetInterfaces().Contains(type)) continue;

                        yield return type.Name;
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
        {
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (!baseType.IsAssignableFrom(type) || type.IsInterface || type.IsAbstract) continue;

                yield return type;
            }
        }

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
