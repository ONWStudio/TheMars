using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.Event
{
    /// <summary>
    /// .. 추상화 된 인터페이스를 제공합니다
    /// </summary>
    public interface IEventSender
    {
        EventSender EventSender { get; }
    }
}
