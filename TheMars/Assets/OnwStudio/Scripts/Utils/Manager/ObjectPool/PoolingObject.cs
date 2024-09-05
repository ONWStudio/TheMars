// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Onw.Manager.ObjectPool
// {
//     [DisallowMultipleComponent]
//     public abstract class PoolingObject : MonoBehaviour
//     {
//         public string Key { get; protected set; } = null;
//         public bool IsChangeParent { get; protected set; } = false;
//
//         public void Disable()
//         {
//             if (!this || !gameObject || !gameObject.activeSelf) return;
//
//             Destroy(gameObject);
//         }
//         // .. 디스트로이 방지
//         protected void Destroy(GameObject obj)
//         {
//             OnDestroyPoolingObject();
//             obj.SetActive(false);
//             ObjectPool.Instance.PushObjectInPool(this);
//         }
//
//         private void OnApplicationQuit() => Object.Destroy(gameObject);
//         protected virtual void OnDestroyPoolingObject() { }
//         protected abstract void OnEnable();
//         protected abstract void OnDisable();
//     }
// }