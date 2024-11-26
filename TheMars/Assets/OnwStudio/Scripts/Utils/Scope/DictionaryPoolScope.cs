using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Onw.Scope
{
    public struct DictionaryPoolScope<TKey, TValue> : ICollectionScope<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> _pooledDictionary;
        
        public Dictionary<TKey, TValue> Get()
        {
            _pooledDictionary ??= DictionaryPool<TKey, TValue>.Get();
            return _pooledDictionary;
        }
        
        public void Dispose()
        {
            if (_pooledDictionary is null) return;

            _pooledDictionary.Clear();
            DictionaryPool<TKey, TValue>.Release(_pooledDictionary);
            _pooledDictionary = null;
        }
    }
}
