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
            Type baseType = assembly.GetType("BaseSkill");

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
