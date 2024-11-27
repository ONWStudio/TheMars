using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Prototype
{
    internal sealed class ReleaseAddressableInstanceFromAssetReference : ReleaseAddressableInstance
    {
        protected override void Release()
        {
            PrototypeManager.Instance.ReleaseInstanceFromRuntimeKey(_primaryKey);
        }
    }
}