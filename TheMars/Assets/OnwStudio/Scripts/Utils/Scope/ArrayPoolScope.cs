using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Scope
{
    public class ArrayPoolScope<T> : ICollectionScope<T[], T>
    {
        private T[] _pooledArray;

        public T[] Get()
        {
            return _pooledArray;
        }
        
        public void Dispose()
        {
            if (_pooledArray is null) return;

            ArrayPool<T>.Shared.Return(_pooledArray, true);
            _pooledArray = null;
        }

        public ArrayPoolScope(int minimumLength)
        {
            _pooledArray = ArrayPool<T>.Shared.Rent(minimumLength);
        }

        ~ArrayPoolScope()
        {
            Dispose();
        }
    }
}
