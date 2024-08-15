using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.Event
{
    /// <summary>
    /// .. RemoveAllListenr메서드를 제공하지 않는 유니티 이벤트입니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent
    {
        [SerializeField] private UnityEvent _unityEvent = new();

        public void AddListener(UnityAction call) => _unityEvent.AddListener(call);
        public void RemoveListener(UnityAction call) => _unityEvent.RemoveListener(call);
        public void Invoke() => _unityEvent.Invoke();
    }

    [Serializable]
    public sealed class SafeUnityEvent<T0>
    {
        [SerializeField] private UnityEvent<T0> _unityEvent = new();

        public void AddListener(UnityAction<T0> call) => _unityEvent.AddListener(call);
        public void RemoveListener(UnityAction<T0> call) => _unityEvent.RemoveListener(call);
        public void Invoke(T0 item) => _unityEvent.Invoke(item);
    }

    [Serializable]
    public sealed class SafeUnityEvent<T0, T1>
    {
        [SerializeField] private UnityEvent<T0, T1> _unityEvent = new();

        public void AddListener(UnityAction<T0, T1> call) => _unityEvent.AddListener(call);
        public void RemoveListener(UnityAction<T0, T1> call) => _unityEvent.RemoveListener(call);
        public void Invoke(T0 item1, T1 item2) => _unityEvent.Invoke(item1, item2);
    }
    
    [Serializable]
    public sealed class SafeUnityEvent<T0, T1, T2>
    {
        [SerializeField] private UnityEvent<T0, T1, T2> _unityEvent = new();

        public void AddListener(UnityAction<T0, T1, T2> call) => _unityEvent.AddListener(call);
        public void RemoveListener(UnityAction<T0, T1, T2> call) => _unityEvent.RemoveListener(call);
        public void Invoke(T0 item1, T1 item2, T2 item3) => _unityEvent.Invoke(item1, item2, item3);
    }

    [Serializable]
    public sealed class SafeUnityEvent<T0, T1, T2, T3>
    {
        [SerializeField] private UnityEvent<T0, T1, T2, T3> _unityEvent = new();

        public void AddListener(UnityAction<T0, T1, T2, T3> call) => _unityEvent.AddListener(call);
        public void RemoveListener(UnityAction<T0, T1, T2, T3> call) => _unityEvent.RemoveListener(call);
        public void Invoke(T0 item1, T1 item2, T2 item3, T3 item4) => _unityEvent.Invoke(item1, item2, item3, item4);
    }
}
