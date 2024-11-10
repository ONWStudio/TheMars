using System;
using System.Collections;
using System.Collections.Generic;
using Onw.Coroutine;
using TM.Buff.Trigger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace TM.Buff
{
    [Serializable]
    public abstract class TMBuffBase : IDisposable
    {
        [field: SerializeField] protected AssetReferenceSprite IconBackground { get; private set; } = new("Sprites/Buff_BG");
        public virtual Color IconBackgroundColor => Color.white;
        
        protected abstract AssetReferenceSprite IconReference { get; set; }
        
        public event UnityAction<TMBuffBase> OnDestroyBuff
        {
            add => _onDestroyBuff.AddListener(value);
            remove => _onDestroyBuff.RemoveListener(value);
        }

        [SerializeField] private UnityEvent<TMBuffBase> _onDestroyBuff = new();

        public void ApplyBuff()
        {
            ApplyBuffProtected();
            TMBuffManager.Instance.ApplyBuff(this);
        }

        protected abstract void ApplyBuffProtected();
        
        public void Dispose()
        {
            releaseReference(IconBackground);
            releaseReference(IconReference);
            _onDestroyBuff.Invoke(this);

            static void releaseReference(AssetReferenceSprite spriteReference)
            {
                if (spriteReference is null || !spriteReference.IsValid()) return;

                spriteReference.ReleaseAsset();
            }
        }
    }

}
