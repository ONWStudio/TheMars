using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMCardUISystemModules
{
    public interface ITMCardMoveBegin
    {
        void OnMoveBegin();
    }

    public interface ITMCardMoveEnd
    {
        void OnMoveEnd();
    }

    public interface ITMCardMove
    {
        void OnMove();
    }
}