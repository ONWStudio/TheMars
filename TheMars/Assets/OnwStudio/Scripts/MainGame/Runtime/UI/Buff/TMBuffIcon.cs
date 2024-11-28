using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Onw.Attribute;
using Onw.Extensions;
using Onw.ObjectPool;
using TM.Buff;

namespace TM.UI
{
    public class TMBuffIcon : MonoBehaviour, IReturnHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event UnityAction OnPointerEnterEvent
        {
            add => _onPointerEnterEvent.AddListener(value);
            remove => _onPointerEnterEvent.RemoveListener(value);
        }
        
        public event UnityAction OnPointerExitEvent
        {
            add => _onPointerExitEvent.AddListener(value);
            remove => _onPointerExitEvent.RemoveListener(value);
        }
        
        [SerializeField, InitializeRequireComponent] private Image _iconBackground;
        [SerializeField, SelectableSerializeField] private Image _iconFrame;
        [SerializeField, SelectableSerializeField] private Image _icon;

        [SerializeField] private UnityEvent _onPointerEnterEvent = new();
        [SerializeField] private UnityEvent _onPointerExitEvent  = new();

        private AsyncOperationHandle<Sprite> _iconHandle;

        private void OnDestroy()
        {
            resetIcon();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _onPointerEnterEvent.Invoke();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _onPointerExitEvent.Invoke();
        }
        
        public void OnReturnToPool()
        {
            resetIcon();
        }

        public void SetUI(TMBuffBase buff)
        {
            _iconBackground.color = buff.IconBackgroundColor;
            _iconFrame.color = buff.IconBackgroundColor.AdjustSaturation(1.5f);
            
            Addressables.LoadAssetAsync<Sprite>(buff.IconReference).Completed += onLoadedIconSprite;
        }

        private void onLoadedIconSprite(AsyncOperationHandle<Sprite> iconHandle)
        {
            _iconHandle = iconHandle;
            _icon.sprite = _iconHandle.Result;
        }

        private void resetIcon()
        {
            _onPointerEnterEvent.RemoveAllListeners();
            _onPointerExitEvent.RemoveAllListeners();
            
            if (_iconHandle.IsValid())
            {
                Addressables.Release(_iconHandle);
            }
        }
    }
}