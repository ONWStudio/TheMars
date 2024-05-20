using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ReleaseAdressablesInstanceFromAssetReference : ReleaseAddressablesInstance
{
    protected override void Release()
    {
        PrototypeManager.Instance.ReleaseInstanceFromRuntimeKey(_primaryKey);
    }
}
