using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.ServiceLocator
{
    public static class ServiceLocator<T> where T : class
    {
        private static T _service = null;

        public static bool TryGetService(out T service)
        {
            service = _service;

            return service is MonoBehaviour monoBehaviour ? monoBehaviour : service is not null;
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
                    Debug.LogWarning("regist service is null");
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
                    Debug.LogWarning("regist service is null");
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