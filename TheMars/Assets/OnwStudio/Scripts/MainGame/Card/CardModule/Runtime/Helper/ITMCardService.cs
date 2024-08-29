using UnityEngine;
using Onw.Event;

namespace TMCard.Runtime
{
    public interface ITMCardService
    {
        Camera CardSystemCamera { get; }
        TMCardCreator CardCreator { get; }
    }
}