using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class DontDestroy : MonoBehaviour
{
    private void Start() => DontDestroyOnLoad(gameObject);
    private DontDestroy() {}
}