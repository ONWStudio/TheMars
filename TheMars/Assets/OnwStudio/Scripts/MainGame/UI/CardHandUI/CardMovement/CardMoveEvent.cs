using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardMoveBegin
{
    void OnMoveBegin();
}

public interface ICardMoveEnd
{
    void OnMoveEnd();
}

public interface ICardMove
{
    void OnMove();
}
