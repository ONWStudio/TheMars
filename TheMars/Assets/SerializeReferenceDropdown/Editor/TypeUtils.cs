using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace SerializeReferenceDropdown.Editor
{
    public static class TypeUtils
    {
        private const string ArrayPropertySubstring = ".Array.data[";
        private static Dictionary<AppDomain, List<Type>> CachedDomainTypes = new Dictionary<AppDomain, List<Type>>();

        public static Type ExtractTypeFromString(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            string[] splitFieldTypename = typeName.Split(' ');
            string assemblyName = splitFieldTypename[0];
            string subStringTypeName = splitFieldTypename[1];
            if (splitFieldTypename.Length > 2)
            {
                subStringTypeName = typeName.Substring(assemblyName.Length + 1);
            }

            Assembly assembly = Assembly.Load(assemblyName);
            Type targetType = assembly.GetType(subStringTypeName);
            return targetType;
        }

        public static bool IsFinalAssignableType(Type type)
        {
            return type.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }

        public static bool IsArrayElement(this SerializedProperty property)
        {
            return property.propertyPath.Contains(ArrayPropertySubstring);
        }

        public static SerializedProperty GetArrayPropertyFromArrayElement(SerializedProperty property)
        {
            string path = property.propertyPath;
            int startIndexArrayPropertyPath = path.IndexOf(ArrayPropertySubstring);
            string propertyPath = path.Remove(startIndexArrayPropertyPath);
            return property.serializedObject.FindProperty(propertyPath);
        }


        public static IEnumerable<Type> GetAllTypesInCurrentDomain()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            if (CachedDomainTypes.TryGetValue(currentDomain, out List<Type> cachedTypes))
            {
                return cachedTypes;
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            CachedDomainTypes.Add(currentDomain, types);

            return types;
        }

        public static Type GetConcreteGenericType(Type propertyType, Type genericType)
        {
            if (propertyType.IsGenericType && CanCreateDirectGenericType())
            {
                Type genericConcreteType = genericType.MakeGenericType(propertyType.GetGenericArguments());
                return genericConcreteType;
            }

            return null;

            bool CanCreateDirectGenericType()
            {
                Type[] genericArguments = genericType.GetInterfaces();
                int interfaceIndex = Array.FindIndex(genericArguments,
                    argType => argType.IsGenericType &&
                               argType.GetGenericTypeDefinition() == propertyType.GetGenericTypeDefinition());
                bool isHaveSameArgumentsCount =
                    propertyType.GetGenericArguments().Length == genericType.GetGenericArguments().Length &&
                    interfaceIndex != -1;
                bool anyAbstract = propertyType.GetGenericArguments().Any(t => t.IsAbstract);
                return isHaveSameArgumentsCount && anyAbstract == false;
            }
        }

        private static Type[] GetBuiltInUnitySerializeTypes()
        {
            return GetDefaultTypes();
        }

        private static Type[] GetDefaultTypes()
        {
            return new[]
            {
                typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int),
                typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(string),

                typeof(Color), typeof(Color32), typeof(Vector2), typeof(Vector3), typeof(Vector4), typeof(Quaternion),
                typeof(Ray), typeof(Ray2D)
            };
        }

        private static IReadOnlyList<Type> SystemObjectTypes;


        public static IReadOnlyList<Type> GetAllSystemObjectTypes()
        {
            if (SystemObjectTypes == null)
            {
                UnityEditor.Compilation.Assembly[] assemblies = CompilationPipeline.GetAssemblies();
                string[] playerAssemblies = assemblies.Where(t => t.flags.HasFlag(AssemblyFlags.EditorAssembly) == false)
                    .Select(t => t.name).ToArray();
                Type baseType = typeof(object);
                TypeCache.TypeCollection typesCollection = TypeCache.GetTypesDerivedFrom(baseType);
                IOrderedEnumerable<Type> customTypes = typesCollection.Where(IsValidTypeForGenericParameter).OrderBy(t => t.FullName);

                List<Type> typesList = new List<Type>();
                typesList.AddRange(GetBuiltInUnitySerializeTypes());
                typesList.AddRange(customTypes);
                SystemObjectTypes = typesList.ToArray();

                bool IsValidTypeForGenericParameter(Type t)
                {
                    bool isUnityObjectType = t.IsSubclassOf(typeof(UnityEngine.Object));

                    bool isFinalSerializeType = !t.IsAbstract && !t.IsInterface && !t.IsGenericType && t.IsSerializable;
                    bool isEnum = t.IsEnum;
                    bool isTargetType = playerAssemblies.Any(asm => t.Assembly.FullName.StartsWith(asm)) ||
                                        t.Assembly.FullName.StartsWith(nameof(UnityEngine));

                    return isTargetType && (isFinalSerializeType || isEnum || isUnityObjectType);
                }
            }

            return SystemObjectTypes;
        }
    }
}