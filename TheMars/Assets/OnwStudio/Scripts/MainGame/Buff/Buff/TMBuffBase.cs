using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Event;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace TM.Buff
{
    public interface IAccrueDayNotifier
    {
        IReadOnlyReactiveField<int> AccrueDay { get; }
    }

    [Serializable]
    public abstract class TMBuffBase : IDisposable
    {
        [field: SerializeField] protected AssetReferenceSprite IconBackground { get; private set; } = new("Sprites/Buff_BG");
        public virtual Color IconBackgroundColor => Color.white;

        protected abstract AssetReferenceSprite IconReference { get; set; }

        private bool _disposed = false;

        public event UnityAction<TMBuffBase> OnDestroyBuff
        {
            add => _onDestroyBuff?.AddListener(value);
            remove => _onDestroyBuff?.RemoveListener(value);
        }

        [SerializeField] private UnityEvent<TMBuffBase> _onDestroyBuff = new();

        public void ApplyBuff()
        {
            if (_disposed) return;

            ApplyBuffProtected();
            TMBuffManager.Instance.ApplyBuff(this);
        }

        protected abstract void ApplyBuffProtected();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  // 소멸자 호출을 방지
        }

        // Dispose 패턴 구현
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // 관리 리소스 해제
                releaseReference(IconBackground);
                releaseReference(IconReference);
                _onDestroyBuff?.Invoke(this);
                _onDestroyBuff?.RemoveAllListeners();
                _onDestroyBuff = null;
            }

            _disposed = true;
            
            static void releaseReference(AssetReferenceSprite spriteReference)
            {
                if (spriteReference is null || !spriteReference.IsValid()) return;

                spriteReference.ReleaseAsset();
            }
        }

        ~TMBuffBase()
        {
            Dispose(false);  // 소멸자는 Dispose(false)로 호출
        }
    }
}