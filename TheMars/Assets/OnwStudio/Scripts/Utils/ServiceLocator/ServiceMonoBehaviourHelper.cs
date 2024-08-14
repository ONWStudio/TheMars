using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Onw.ServiceLocator
{
    public static class ServiceMonoBehaviourHelper
    {
        public static T GetService<T>() where T : MonoBehaviour
        {
            if (ServiceLocator<T>.TryGetService(out var service)) return service;
            
            var scene = SceneManager.GetActiveScene();
            var gameObjects = scene.GetRootGameObjects();

            foreach (var go in gameObjects)
            {
                service = go.GetComponentInChildren<T>();

                if (service != null)
                {
                    break;
                }
            }

            if (service == null)
            {
                service = new GameObject(typeof(T).Name).AddComponent<T>();
            }

            ServiceLocator<T>.RegisterService(service);

            return service;
        }
    }
}
