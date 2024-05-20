using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionHelper
{
    public static IEnumerable<T> GetInterfacesFromType<T>()
    {
        Type interfaceType = typeof(T);
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("T는 interface가 될 수 없습니다");
        }

        List<T> implementingTypes = new();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!interfaceType.IsAssignableFrom(type) || type.IsInterface || type.IsAbstract) continue;

                T instance = (T)Activator.CreateInstance(type);
                implementingTypes.Add(instance);
            }
        }

        return implementingTypes;
    }

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
