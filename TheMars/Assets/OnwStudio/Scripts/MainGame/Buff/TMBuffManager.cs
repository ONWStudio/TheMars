using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using Onw.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TM.Buff
{
    public sealed class TMBuffManager : SceneSingleton<TMBuffManager>
    {
        protected override string SceneName => "MainGameScene";

        public event UnityAction<TMBuffBase> OnAddedBuff
        {
            add => _onAddedBuff.AddListener(value);
            remove => _onAddedBuff.RemoveListener(value);
        }
        
        public event UnityAction<TMBuffBase> OnRemovedBuff
        {
            add => _onRemovedBuff.AddListener(value);
            remove => _onRemovedBuff.RemoveListener(value);
        }
        
        public IReadOnlyList<TMBuffBase> Buffs => _buffs;
        
        [SerializeReference, SerializeReferenceDropdown, ReadOnly] private List<TMBuffBase> _buffs = new();

        [SerializeField] private UnityEvent<TMBuffBase> _onAddedBuff = new();
        [SerializeField] private UnityEvent<TMBuffBase> _onRemovedBuff = new();

        public void ApplyBuff(TMBuffBase buffBase)
        {
            _buffs.Add(buffBase);
            _onAddedBuff.Invoke(buffBase);

            buffBase.OnDestroyBuff += onDestroyBuff;

            void onDestroyBuff(TMBuffBase destroyedBuff)
            {
                destroyedBuff.OnDestroyBuff -= onDestroyBuff;
                
                if (_buffs.Remove(destroyedBuff))
                {
                    _onRemovedBuff.Invoke(destroyedBuff);
                }
            }
        }
        
        protected override void Init()
        {
        }

        private void OnDestroy()
        {
            _buffs.ForEach(buff => buff.Dispose());
        }
    }
}
