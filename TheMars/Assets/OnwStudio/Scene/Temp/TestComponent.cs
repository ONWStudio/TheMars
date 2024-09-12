using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    private void Start()
    {
        TestAwake testAwake = gameObject.AddComponent<TestAwake>();
        Debug.Log("Start End");
    }
}
