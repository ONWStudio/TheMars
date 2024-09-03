using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TM
{
    public sealed class CardCollectNotifyIcon : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private Button _collecCardButton;

        private void Start()
        {
            // _collecCardButton.onClick.AddListener();
        }
    }
}
