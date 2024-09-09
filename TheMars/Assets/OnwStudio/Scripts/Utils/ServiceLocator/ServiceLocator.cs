using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ServiceLocator
{
    /// <summary>
    /// .. 싱글톤을 대체가능한 서비스 중개자 입니다
    /// 싱글톤의 전역접근 가능한 기능을 대체할 수 있습니다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ServiceLocator<T> where T : class
    {
        private static T _service = null;
        
        // .. 등록이 되어있지 않은 경우가 있기때문에 TryGet메서드만 제공합니다
        public static bool TryGetService(out T service)
        {
            service = _service;

            return service as MonoBehaviour ?? service is not null;
        }

        public static void InvokeService(Action<T> onResultService)
        {
            if (!TryGetService(out T service)) return;
            
            onResultService?.Invoke(service);
        }
        
        public static bool RegisterService(T service)
        {
            if (_service is MonoBehaviour monoService)
            {
                if (monoService)
                {
                    Debug.LogWarning("service is not null, please use ChangeService Method");

                    return false;
                }

                if (service as MonoBehaviour == null)
                {
                    Debug.LogWarning("register service is null");
                    return false;
                }
            }
            else
            {
                if (_service is not null)
                {
                    Debug.LogWarning("service is not null, please use ChangeService Method");
                    return false;
                }

                if (service is null)
                {
                    Debug.LogWarning("register service is null");
                    return false;
                }
            }

            _service = service;
            return true;
        }

        public static void ClearService()
        {
            _service = null;
        }

        public static bool IsEqual(T service)
        {
            return _service == service;
        }

        public static void ChangeService(T newService)
        {
            if (newService is null || (newService is MonoBehaviour monoService && !monoService))
            {
                Debug.LogWarning("newService is null");
            }

            _service = newService;
        }
    }
}