using Onw.Attribute;
using Onw.Manager;
using UnityEngine;
namespace TM
{
    public sealed class PlayerManager : SceneSingleton<PlayerManager>
    {
        public override string SceneName => "MainGameScene";

        [field: SerializeField, ReadOnly] public int Tera { get; set; }
        [field: SerializeField, ReadOnly] public int MarsLithum { get; set; }

        protected override void Init()
        {
        }
    }
}
