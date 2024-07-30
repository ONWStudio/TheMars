using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;
using Onw.Attribute;

namespace TM
{
    public sealed class PlayerManager : SceneSingleton<PlayerManager>
    {
        public override string SceneName => "MainGameScene";

        [field: SerializeField, ReadOnly] public int Tera { get; set; } = 0;
        [field: SerializeField, ReadOnly] public int MarsLithum { get; set; } = 0;

        protected override void Init()
        {
        }
    }
}
