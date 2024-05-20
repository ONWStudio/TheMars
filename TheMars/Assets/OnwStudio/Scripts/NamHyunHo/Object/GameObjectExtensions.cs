using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void Destroy<T, R>(this T _, R poolingObject) where T : Component where R : PoolingObject
        => poolingObject.Disable();
    public static T Instantiate<T>(this Component requestObject, T original) where T : PoolingObject
        => ObjectPool.Instance.PopOrCreate<T>(() => Object.Instantiate(original));
    public static T Instantiate<T>(this Component requestObject, T original, Transform parent) where T : PoolingObject
        => ObjectPool.Instance.PopOrCreate<T>(() => Object.Instantiate(original, parent));
}
