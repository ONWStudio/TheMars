using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.Components
{
    public interface IOnwMouseInputEvent
    {
        event UnityAction<Vector2> OnDownEvent;
        event UnityAction<Vector2> OnUpEvent;
        
        void Update(MouseInputEvent owner);
    }
}
