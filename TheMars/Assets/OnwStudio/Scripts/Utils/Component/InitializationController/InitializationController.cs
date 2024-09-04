using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Onw.Components.Initialization
{
    public sealed class InitializationController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _gameObjectToInitialize;
        
        private void Awake()
        {
            foreach (IInitialization initialization in _gameObjectToInitialize.SelectMany(go => go.GetComponents<MonoBehaviour>()).OfType<IInitialization>())
            {
                initialization.Initialize();
            }
        }
    }
}
