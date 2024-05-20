using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpdatingClass
{
    protected internal abstract void Update();
    protected UpdatingClass() => UpdateManager.Instance.UpdatingMember.Add(this);
}

public sealed class UpdateManager : Singleton<UpdateManager> // .. 모노 헤이비어의 업데이트 기능만을 이용하고싶은 네이티브 클래스들을 위해 기능하는 매니저
{
    public List<UpdatingClass> UpdatingMember { get; } = new List<UpdatingClass>();

    protected override void Init() {}
    private void Update() => UpdatingMember.ForEach(member => member.Update());
}

