using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Onw.Manager.ObjectPool
{
    using Extensions;

    public sealed class ObjectPool
    {
        public static ObjectPool Instance { get; } = new();

        private readonly Dictionary<string, Stack<PoolingObject>> _pool = new Dictionary<string, Stack<PoolingObject>>();
        private GameObject _poolParent;

        public T Pop<T>(string key = null) where T : PoolingObject
        {
            key ??= typeof(T).Name;
            Stack<PoolingObject> objects = getObjects(key);

            T poolingObject = objects.Count > 0 ? objects.Pop() as T : null;

            if (poolingObject)
            {
                poolingObject.gameObject.SetActive(true);
            }

            return poolingObject;
        }

        public T PopSetParent<T>(Transform parent, string key = null) where T : PoolingObject
        {
            T poolingObject = Pop<T>(key);

            if (poolingObject)
            {
                poolingObject.transform.SetParent(parent);
            }

            return poolingObject;
        }

        public GameObject PopToObject(string key)
        {
            Stack<PoolingObject> objects = getObjects(key);

            PoolingObject poolingObject = objects.Count > 0 ? objects.Pop() : null;

            if (poolingObject)
            {
                poolingObject.gameObject.SetActive(true);
            }

            return poolingObject.gameObject;
        }

        public GameObject PopSetParentToObject(string key, Transform parent)
        {
            GameObject poolingObject = PopToObject(key);

            if (poolingObject)
            {
                poolingObject.transform.SetParent(parent);
            }

            return poolingObject;
        }

        public T PopOrCreate<T>(Func<PoolingObject> createMethod, string key = null) where T : PoolingObject
        {
            key ??= typeof(T).Name;
            Stack<PoolingObject> objects = getObjects(key);

            T poolingObject = (objects.Count > 0 ? objects.Pop() : createMethod?.Invoke()) as T;
            poolingObject.gameObject.SetActive(true);

            return poolingObject;
        }

        public T PopOrCreateSetParent<T>(Func<PoolingObject> createMethod, Transform parentTransform, string key = null) where T : PoolingObject
        {
            key ??= typeof(T).Name;
            Stack<PoolingObject> objects = getObjects(key);

            T poolingObject = (objects.Count > 0 ? objects.Pop() : createMethod?.Invoke()) as T;

            poolingObject.transform.SetParent(parentTransform);
            poolingObject.gameObject.SetActive(true);

            return poolingObject;
        }

        public GameObject PopOrCreateSetParentToObject(Func<PoolingObject> createMethod, Transform parentTransform, string key)
        {
            Stack<PoolingObject> objects = getObjects(key);

            GameObject gameObject = (objects.Count > 0 ? objects.Pop() : createMethod?.Invoke())?.gameObject;
            gameObject.transform.SetParent(parentTransform);
            gameObject.SetActive(true);

            return gameObject;
        }

        public GameObject PopOrCreateToObject(Func<PoolingObject> createMethod, string key)
        {
            Stack<PoolingObject> objects = getObjects(key);

            GameObject gameObject = (objects.Count > 0 ? objects.Pop() : createMethod?.Invoke()).gameObject;
            gameObject.SetActive(true);

            return gameObject;
        }

        public void PushObjectInPool<T>(T poolingObject) where T : PoolingObject
        {
            if (poolingObject.IsChangeParent) poolingObject.transform.SetParent(_poolParent.transform);

            Stack<PoolingObject> objects = getObjects(poolingObject.Key ?? poolingObject.GetType().Name);
            objects.Push(poolingObject);
        }

        private Stack<PoolingObject> getObjects(string key)
        {
            if (!_pool.TryGetValue(key, out Stack<PoolingObject> objects))
            {
                objects = new Stack<PoolingObject>();
                _pool.Add(key, objects);
            }

            return objects;
        }

        private ObjectPool()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                UnityEngine.Object.Destroy(_poolParent);
                _pool.ForEach(objects => objects.Value.ForEach(obj => UnityEngine.Object.Destroy(obj)));

                _pool.Clear();
                _poolParent = new GameObject("Pool");
            };

            _poolParent = new GameObject("Pool");
        }
    }
}