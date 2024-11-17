using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ErrorTestComponent : MonoBehaviour
{
    private void Awake()
    {
        RectTransform[] rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rectTransform in rectTransforms)
        {
            Debug.Log($"{rectTransform.gameObject.name} : {rectTransform.anchoredPosition}, {rectTransform.anchorMin}, {rectTransform.anchorMax}, {rectTransform.pivot}");
        }
    }
}
