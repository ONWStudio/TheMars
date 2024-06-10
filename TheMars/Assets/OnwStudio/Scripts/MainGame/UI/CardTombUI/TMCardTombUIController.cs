using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class TMCardTombUIController : MonoBehaviour
{
    [SerializeField] private List<TMCardUIController> _deadCards = new();
}
