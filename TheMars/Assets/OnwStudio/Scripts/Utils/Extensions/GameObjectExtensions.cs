using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Extensions
{
    using Manager.ObjectPool;
    using Object = UnityEngine.Object;

    public static class GameObjectExtensions
    {
        public static void Destroy<T, R>(this T _, R poolingObject) where T : Component where R : PoolingObject
            => poolingObject.Disable();
        public static T Instantiate<T>(this Component requestObject, T original) where T : PoolingObject
            => ObjectPool.Instance.PopOrCreate<T>(() => Object.Instantiate(original));
        public static T Instantiate<T>(this Component requestObject, T original, Transform parent) where T : PoolingObject
            => ObjectPool.Instance.PopOrCreate<T>(() => Object.Instantiate(original, parent));

        public static void SetPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetPositionX(x);
        }

        public static void SetPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetPositionY(y);
        }

        public static void SetPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetPositionZ(z);
        }
    }
}
