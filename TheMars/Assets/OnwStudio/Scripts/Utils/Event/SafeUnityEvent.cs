using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.Event
{
    public interface IUnityEventListenerModifier
    {
        void AddListener(in UnityAction call);
        void RemoveListener(in UnityAction call);
    }

    public interface IUnityEventListenerModifier<T>
    {
        void AddListener(in UnityAction<T> call);
        void RemoveListener(in UnityAction<T> call);
    }

    public interface IUnityEventListenerModifier<T0, T1>
    {
        void AddListener(in UnityAction<T0, T1> call);
        void RemoveListener(in UnityAction<T0, T1> call);
    }

    public interface IUnityEventListenerModifier<T0, T1, T2>
    {
        void AddListener(in UnityAction<T0, T1, T2> call);
        void RemoveListener(in UnityAction<T0, T1, T2> call);
    }

    public interface IUnityEventListenerModifier<T0, T1, T2, T3>
    {
        void AddListener(in UnityAction<T0, T1, T2, T3> call);
        void RemoveListener(in UnityAction<T0, T1, T2, T3> call);
    }
    
    /// <summary>
    /// .. RemoveAllListener메서드를 제공하지 않는 유니티 이벤트입니다
    /// 딜리게이트에 대한 참조를 관리하지 않는 경우 메모리 누수와 예기치 못한 버그가 발생할 수 있습니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent : IUnityEventListenerModifier
    {
        [SerializeField] private UnityEvent _unityEvent = new();

        public void AddListener(in UnityAction call) => _unityEvent.AddListener(call);
        public void RemoveListener(in UnityAction call) => _unityEvent.RemoveListener(call);
        public void Invoke() => _unityEvent.Invoke();
    }

    /// <summary>
    /// .. RemoveAllListener메서드를 제공하지 않는 유니티 이벤트입니다
    /// 딜리게이트에 대한 참조를 관리하지 않는 경우 메모리 누수와 예기치 못한 버그가 발생할 수 있습니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent<T0> : IUnityEventListenerModifier<T0>
    {
        [SerializeField] private UnityEvent<T0> _unityEvent = new();

        public void AddListener(in UnityAction<T0> call) => _unityEvent.AddListener(call);
        public void RemoveListener(in UnityAction<T0> call) => _unityEvent.RemoveListener(call);
        public void Invoke(in T0 item) => _unityEvent.Invoke(item);
    }

    /// <summary>
    /// .. RemoveAllListener메서드를 제공하지 않는 유니티 이벤트입니다
    /// 딜리게이트에 대한 참조를 관리하지 않는 경우 메모리 누수와 예기치 못한 버그가 발생할 수 있습니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent<T0, T1> : IUnityEventListenerModifier<T0, T1>
    {
        [SerializeField] private UnityEvent<T0, T1> _unityEvent = new();

        public void AddListener(in UnityAction<T0, T1> call) => _unityEvent.AddListener(call);
        public void RemoveListener(in UnityAction<T0, T1> call) => _unityEvent.RemoveListener(call);
        public void Invoke(in T0 item1, in T1 item2) => _unityEvent.Invoke(item1, item2);
    }
    
    /// <summary>
    /// .. RemoveAllListener메서드를 제공하지 않는 유니티 이벤트입니다
    /// 딜리게이트에 대한 참조를 관리하지 않는 경우 메모리 누수와 예기치 못한 버그가 발생할 수 있습니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent<T0, T1, T2> : IUnityEventListenerModifier<T0, T1, T2>
    {
        [SerializeField] private UnityEvent<T0, T1, T2> _unityEvent = new();

        public void AddListener(in UnityAction<T0, T1, T2> call) => _unityEvent.AddListener(call);
        public void RemoveListener(in UnityAction<T0, T1, T2> call) => _unityEvent.RemoveListener(call);
        public void Invoke(in T0 item1, in T1 item2, in T2 item3) => _unityEvent.Invoke(item1, item2, item3);
    }

    /// <summary>
    /// .. RemoveAllListener메서드를 제공하지 않는 유니티 이벤트입니다
    /// 딜리게이트에 대한 참조를 관리하지 않는 경우 메모리 누수와 예기치 못한 버그가 발생할 수 있습니다
    /// </summary>
    [Serializable]
    public sealed class SafeUnityEvent<T0, T1, T2, T3> : IUnityEventListenerModifier<T0, T1, T2, T3>
    {
        [SerializeField] private UnityEvent<T0, T1, T2, T3> _unityEvent = new();

        public void AddListener(in UnityAction<T0, T1, T2, T3> call) => _unityEvent.AddListener(call);
        public void RemoveListener(in UnityAction<T0, T1, T2, T3> call) => _unityEvent.RemoveListener(call);
        public void Invoke(in T0 item1, in T1 item2, in T2 item3, in T3 item4) => _unityEvent.Invoke(item1, item2, item3, item4);
    }
}
