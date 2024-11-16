using Onw.Attribute;
using Onw.Extensions;
using Onw.Manager.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using TM.Buff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace TM.UI
{
    public class TMBuffIcon : MonoBehaviour, IReturnHandler
    {
        [SerializeField, InitializeRequireComponent] private Image _iconBackground;
        [SerializeField, SelectableSerializeField] private Image _iconFrame;
        [SerializeField, SelectableSerializeField] private Image _icon;

        private AsyncOperationHandle<Sprite> _iconHandle;

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

        private void OnDestroy()
        {
            if (!_iconHandle.IsValid()) return;

            Addressables.Release(_iconHandle);
        }

        public void OnReturnToPool()
        {
            if (!_iconHandle.IsValid()) return;

            Addressables.Release(_iconHandle);
            _icon.sprite = null;
        }
    }
}