using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Onw.VContainerUtils
{
    /// <summary>
    /// .. 인위적으로 어떤 object에 대해 Inject 시킬 경우 타겟이 해당 인터페이스를 구현하고 있을경우 PostInject메서드를 호출합니다
    /// 해당 메서드를 호출하려면 IObjectResolver의 확장 메서드인 InjectOnPost 메서드를 사용해야합니다
    /// </summary>
    public interface IPostInject
    {
        void PostInject(IObjectResolver container);
    }
}

