using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionHelper
{
    public static IEnumerable<T> GetChildClassesFromType<T>() where T : class
         => AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => Activator.CreateInstance(type) as T);

    public static IEnumerable<string> GetClassNamesFromParent(string baseClass)
    {
        IEnumerable<string> childClassNames = null;

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type baseType = assembly.GetType(baseClass);

            if (baseType is not null)
            {
                childClassNames = assembly
                    .GetTypes()
                    .Where(type => type.IsSubclassOf(baseType))
                    .Select(type => type.Name);

                break;
            }
        }

        return childClassNames;
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
        IEnumerable<string> enumValues = null;
        bool isFind = false;

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (isFind) break;

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsEnum || type.Name != enumTypeName) continue;

                enumValues = Enum.GetNames(type);
                isFind = true;
            }
        }

        return enumValues;
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
