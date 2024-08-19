﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.KeysGenerators
{
    public static class KeyListGeneratorCache
    {
        private static List<KeyListGeneratorData> _populators;
        private static Dictionary<Type, List<KeyListGeneratorData>> _populatorsByType;

        static KeyListGeneratorCache()
        {
            _populators = new List<KeyListGeneratorData>();
            _populatorsByType = new Dictionary<Type, List<KeyListGeneratorData>>();
            TypeCache.TypeCollection populatorTypes = TypeCache.GetTypesDerivedFrom<KeyListGenerator>();
            foreach (Type populatorType in populatorTypes.Where(x => !x.IsAbstract))
            {
                IEnumerable<KeyListGeneratorAttribute> attributes = populatorType.GetCustomAttributes<KeyListGeneratorAttribute>();
                foreach (KeyListGeneratorAttribute attribute in attributes)
                    _populators.Add(new KeyListGeneratorData(attribute.Name, attribute.TargetType, populatorType, attribute.NeedsWindow));
            }
        }

        public static IReadOnlyList<KeyListGeneratorData> GetPopulatorsForType(Type type)
        {
            if (!_populatorsByType.ContainsKey(type))
                _populatorsByType.Add(type, new List<KeyListGeneratorData>(_populators.Where(x => x.TargetType.IsAssignableFrom(type))));
            return _populatorsByType[type];
        }
    }
}