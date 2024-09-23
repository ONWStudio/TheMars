using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using Onw.Components.Movement;

namespace TM.Runtime.UI
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
            
            _collectCardButton.onClick.AddListener(() =>
            {
                TMCardCollectUIController.Instance.ActiveUI();
                GenericObjectPool<TMCardCollectNotifyIcon>.Return(this);
            });
        }
        
        public void SetTargetLocalPosition(Vector3 targetPosition)
        {
            _smoothMover.TargetPosition = targetPosition;
        }
    }
}
