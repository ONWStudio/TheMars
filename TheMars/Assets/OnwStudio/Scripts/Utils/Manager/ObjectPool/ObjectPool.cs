using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Manager.ObjectPool
{
    using Extensions;

    public interface IPooledObject
    {
        void OnPopFromPool();
        void OnReturnToPool();
    }

    public static class GenericObjectPool<T> where T : Component
    {
        private static readonly Stack<T> _pool = new();
        private static readonly GameObject _poolParent;

        public static bool TryPop(out T genericComponent)
        {
            genericComponent = null;
            if (_pool.TryPop(out genericComponent) && genericComponent)
            {
                genericComponent.transform.SetParent(null, false);
                genericComponent.gameObject.SetActive(true);
                
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (genericComponent is IPooledObject pooledObject)
                {
                    pooledObject.OnPopFromPool();
                }
                
                return true;
            }
            return false;
        }

        public static T Pop()
        {
            if (!_pool.TryPop(out T genericComponent) || !genericComponent) return null;

            genericComponent.transform.SetParent(null, false);
            genericComponent.gameObject.SetActive(true);
            
            if (genericComponent is IPooledObject pooledObject)
            {
                pooledObject.OnPopFromPool();
            }
            
            return genericComponent;
        }

        public static void Return(T genericComponent)
        {
            if (!genericComponent) return;
            
            if (genericComponent is IPooledObject pooledObject)
            {
                pooledObject.OnReturnToPool();
            }
            
            genericComponent.gameObject.SetActive(false);
            genericComponent.transform.SetParent(_poolParent.transform, false);
            
            _pool.Push(genericComponent);
        }

        public static void ReleaseAllObject()
        {
            _pool.ForEach(component => UnityEngine.Object.Destroy(component.gameObject));
            _pool.Clear();
        }
        
        static GenericObjectPool()
        {
            _poolParent = new(nameof(GenericObjectPool<T>));
        }
    }
    
    public static class KeyedObjectPool
    {
        private static readonly Dictionary<string, Stack<GameObject>> _pool = new();
        private static readonly GameObject _poolParent;

        public static bool TryPop(string key, out GameObject pooledObject)
        {
            pooledObject = null;
            
            if (_pool.TryGetValue(key, out Stack<GameObject> stack))
            {
                while (stack.TryPop(out pooledObject))
                {
                    if (!pooledObject) continue;
                    
                    pooledObject.transform.SetParent(null, false);
                    pooledObject.SetActive(true);
                    return true;
                }
            }

            return false;
        }
        
        public static GameObject Pop(string key)
        {
            if (!_pool.TryGetValue(key, out Stack<GameObject> stack) || stack.Count <= 0) return null;

            while (stack.TryPop(out GameObject pooledObject))
            {
                if (!pooledObject) continue;

                pooledObject.transform.SetParent(null, false);
                pooledObject.SetActive(true);

                return pooledObject;
            }

            return null;
        }
        
        public static void Return(string key, GameObject objectToReturn)
        {
            if (string.IsNullOrEmpty(key) || !objectToReturn) return;
            
            if (!_pool.TryGetValue(key, out Stack<GameObject> stack))
            {
                stack = new();
                _pool.Add(key, stack);
            }

            objectToReturn.SetActive(false);
            objectToReturn.transform.SetParent(_poolParent.transform, false);
            stack.Push(objectToReturn);
        }
        
        public static void ReleaseAllObject()
        {
            foreach (GameObject o in _pool.Values.SelectMany(stack => stack).Where(obj => obj))
            {
                UnityEngine.Object.Destroy(o);
            }

            _pool.Clear();
        }
        
        static KeyedObjectPool()
        {
            _poolParent = new(nameof(KeyedObjectPool));
        }
    }
    
    public static class GenericKeyedObjectPool<T> where T : Component
    {
        private static readonly Dictionary<string, Stack<T>> _pool = new();
        private static readonly GameObject _poolParent;

        public static bool TryPop(string key, out T genericComponent)
        {
            genericComponent = null;
            if (_pool.TryGetValue(key, out Stack<T> stack))
            {
                while (stack.TryPop(out genericComponent))
                {
                    if (!genericComponent) continue;
                    
                    genericComponent.transform.SetParent(null, false);
                    genericComponent.gameObject.SetActive(true);

                    if (genericComponent is IPooledObject pooledObject)
                    {
                        pooledObject.OnPopFromPool();
                    }
                    
                    return true;
                }
            }
            
            return false;
        }
        
        public static T Pop(string key)
        {
            if (!_pool.TryGetValue(key, out Stack<T> stack) || stack.Count <= 0) return null;

            while (stack.TryPop(out T genericComponent))
            {
                if (!genericComponent) continue;

                genericComponent.transform.SetParent(null, false);
                genericComponent.gameObject.SetActive(true);

                if (genericComponent is IPooledObject pooledObject)
                {
                    pooledObject.OnPopFromPool();
                }
                
                return genericComponent;
            }

            return null;
        }
        
        public static void Return(string key, T objectToReturn)
        {
            if (string.IsNullOrEmpty(key) || !objectToReturn) return;
            
            if (!_pool.TryGetValue(key, out Stack<T> stack))
            {
                stack = new();
                _pool.Add(key, stack);
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (objectToReturn is IPooledObject pooledObject)
            {
                pooledObject.OnReturnToPool();
            }
            
            objectToReturn.gameObject.SetActive(false);
            objectToReturn.transform.SetParent(_poolParent.transform, false);
            stack.Push(objectToReturn);
        }
        
        public static void ReleaseAllObject()
        {
            foreach (T o in _pool.Values.SelectMany(stack => stack).Where(obj => obj))
            {
                UnityEngine.Object.Destroy(o);
            }

            _pool.Clear();
        }
        
        static GenericKeyedObjectPool()
        {
            _poolParent = new(nameof(GenericObjectPool<T>));
        }
    }
}