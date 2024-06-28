using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Onw.Dispatcher
{
    /// <summary>
    /// .. 비동기적 처리를 하는 경우 특정 결과를 메인스레드에서 처리하도록 도와주는 디스패처 클래스입니다
    /// Editor와 Runtime 모두 처리 가능합니다
    /// </summary>
    public static class UnitySyncContextDispatcher
    {
        private static readonly Queue<Action> _executionQueue = new();
        private static SynchronizationContext _unityContext;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Debug.Log("Run Unity SyncContext Dispatcher");
            _unityContext = SynchronizationContext.Current;
            Application.quitting += () => _unityContext = null;
        }

        private static void ExecuteActions()
        {
#if DEBUG
            Debug.Log("Execute Actions");
#endif

            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        public static void Enqueue(Action action)
        {
            if (_unityContext == null)
            {
                throw new InvalidOperationException("UnitySyncContextDispatcher 가 초기화 되어있지 않습니다");
            }

            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
                _unityContext.Post(_ => ExecuteActions(), null);
            }
        }
    }
}
