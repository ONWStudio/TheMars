using System.Collections;
using UnityEngine;

namespace Onw
{
    [DisallowMultipleComponent]
    public class DontDestroy : MonoBehaviour
    {
        private void Start() => DontDestroyOnLoad(gameObject);
        private DontDestroy() { }
    }
}