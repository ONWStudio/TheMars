using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using Onw.Components.Movement;
using Onw.VContainerUtils;
using TM.Card.UI;

namespace TM
{
    public sealed class TMCardCollectNotifyIcon : MonoBehaviour, IPostInject
    {
        [Header("Button")]
        [SerializeField, InitializeRequireComponent] private Button _collectCardButton;
        
        [Header("Option")]
        [SerializeField, InitializeRequireComponent] private Vector2SmoothMover _smoothMover;

        [SerializeField, Inject] private TMCardCollectUIController _collectUIController; 
        
        private void Start()
        {
            _smoothMover.IsLocal = true;
        }
        
        public void SetTargetLocalPosition(Vector3 targetPosition)
        {
            _smoothMover.TargetPosition = targetPosition;
        }
        
        void IPostInject.PostInject(IObjectResolver container)
        {
            _collectCardButton.onClick.AddListener(() =>
            {
                _collectUIController.ActiveUI();
                GenericObjectPool<TMCardCollectNotifyIcon>.Return(this);
            });
        }
    }
}
