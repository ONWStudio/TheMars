using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Event;

namespace TMCard.Effect
{
    public interface ITMEffectTrigger
    {
        CardEvent OnEffectEvent { get; }
    }

    public interface ITMCardController : IEventSender
    {
        CardEvent OnDrawBeginEvent { get; }
    }
}
