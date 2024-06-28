using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Manager.Prototype
{
    internal sealed class ReleaseAdressablesInstanceFromAssetReference : ReleaseAddressablesInstance
    {
        protected override void Release()
        {
            PrototypeManager.Instance.ReleaseInstanceFromRuntimeKey(_primaryKey);
        }
    }
}