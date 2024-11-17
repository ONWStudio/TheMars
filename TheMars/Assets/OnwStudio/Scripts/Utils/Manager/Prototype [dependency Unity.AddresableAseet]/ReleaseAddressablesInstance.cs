using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Manager.Prototype
{
    [DisallowMultipleComponent]
    internal class ReleaseAddressablesInstance : MonoBehaviour
    {
        protected string _primaryKey = null;

        internal virtual string PrimaryKey
        {
            get => _primaryKey;
            set
            {
                if (_primaryKey is not null)
                {
#if DEBUG
                    Debug.LogWarning("ReleaseAddressablesInstance : 키가 이미 할당되어 있습니다");
#endif
                    return;
                }

                _primaryKey = value;
            }
        }

        protected virtual void Release()
        {
            PrototypeManager.Instance.ReleaseInstance(_primaryKey);
        }

        private void OnDestroy()
        {
            if (_primaryKey is null || !gameObject.scene.isLoaded) return;

            Release();
        }
    }
}