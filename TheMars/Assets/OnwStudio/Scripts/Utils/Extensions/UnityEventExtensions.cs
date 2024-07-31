using System;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.Extensions
{
    public static class UnityEventExtensions
    {
        public static void SetPersistentListenerState(this UnityEventBase unityEvent, UnityEventCallState unityEventCall)
        {
            for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
            {
                unityEvent.SetPersistentListenerState(i, unityEventCall);
            }
        }
    }
}
