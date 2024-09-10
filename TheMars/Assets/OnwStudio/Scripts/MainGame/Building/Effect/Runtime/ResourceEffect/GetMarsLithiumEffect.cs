using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.ServiceLocator;
using TM.Building.Effect.Creator;
using TM.Manager;

namespace TM.Building.Effect
{
    public sealed class GetMarsLithiumEffect : TMBuildingResourceEffect
    {
        protected override void AddResource(PlayerManager player, int resource)
        {
            player.MarsLithium += resource;
        }
    }
}