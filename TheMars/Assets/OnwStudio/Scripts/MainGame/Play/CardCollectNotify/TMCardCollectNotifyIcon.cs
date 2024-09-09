using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using Onw.Components.Movement;
using Onw.ServiceLocator;
using TMCard.UI;

namespace TM
{
    public sealed class TMCardCollectNotifyIcon : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField, InitializeRequireComponent] private Button _collectCardButton;
        
        [Header("Option")]
        [SerializeField, InitializeRequireComponent] private Vector2SmoothMover _smoothMover;
        
        private void Start()
        {
            _smoothMover.IsLocal = true;
            
            _collectCardButton.onClick.AddListener(() 
                => ServiceLocator<TMCardCollectUIController>
                    .InvokeService(collectUIController => collectUIController.ActiveUI()));
        }

        public void SetTargetLocalPosition(Vector3 targetPosition)
        {
            _smoothMover.TargetPosition = targetPosition;
        }
    }
}
