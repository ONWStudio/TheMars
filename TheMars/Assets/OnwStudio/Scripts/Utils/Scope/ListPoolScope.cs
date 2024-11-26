using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Onw.Scope
{
    public struct ListPoolScope<T> : ICollectionScope<List<T>, T>
    {
        private List<T> _pooledList;
        
        public List<T> Get()
        {
            _pooledList ??= ListPool<T>.Get();
            return _pooledList;
        }
        
        public void Dispose()
        {
            if (_pooledList is null) return;
            
            _pooledList.Clear();
            ListPool<T>.Release(_pooledList);
            _pooledList = null;
        }
    }
}
