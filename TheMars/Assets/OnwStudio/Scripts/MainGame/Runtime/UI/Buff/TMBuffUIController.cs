using Codice.Client.Common.GameUI;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using TM.Buff;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace TM.UI
{
    public class TMBuffUIController : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private ScrollRect _buffScrollView;

        private readonly Dictionary<int, TMBuffIcon> _buffDictionary = new();

        private void Start()
        {
            _buffScrollView.content.anchorMin.Set(0, 0.5f);
            _buffScrollView.content.anchorMax.Set(0, 0.5f);
            _buffScrollView.content.pivot = new(0f, 0.5f);

            TMBuffManager.Instance.OnAddedBuff += OnAddedBuff;
            TMBuffManager.Instance.OnRemovedBuff += OnRemovedBuff;
        }

        public void OnAddedBuff(TMBuffBase buff)
        {
            if (!GenericObjectPool<TMBuffIcon>.TryPop(out TMBuffIcon icon))
            {
                icon = new GameObject("Buff_Icon").AddComponent<TMBuffIcon>();
            }

            icon.SetUI(buff);
            _buffDictionary.Add(buff.GetHashCode(), icon);
        }

        public void OnRemovedBuff(TMBuffBase buff)
        {
            if (!_buffDictionary.TryGetValue(buff.GetHashCode(), out TMBuffIcon icon)) return;
            
            _buffDictionary.Remove(buff.GetHashCode());
            GenericObjectPool<TMBuffIcon>.Return(icon);
        }
    }
}