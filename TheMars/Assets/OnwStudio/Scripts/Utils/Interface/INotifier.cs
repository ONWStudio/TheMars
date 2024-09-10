using System.Collections;
using System.Collections.Generic;
using Onw.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.Interface
{
    /// <summary>
    /// .. TODO : 추후 이벤트 타입을 string이 아닌 제네릭으로 이벤트 알림 타입 자체를 클래스화
    /// </summary>
    public interface INotifier
    {
        event UnityAction<string> OnNotifyEvent;
    }
}
