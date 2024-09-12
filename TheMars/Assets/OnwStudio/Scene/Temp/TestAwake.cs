using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAwake : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Test Awake : Awake");
    }

    private void OnEnable()
    {
        Debug.Log("Test Awake : OnEnable");
    }
}
