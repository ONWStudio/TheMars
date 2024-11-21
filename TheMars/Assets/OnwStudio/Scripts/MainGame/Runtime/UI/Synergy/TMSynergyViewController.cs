using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using Onw.Attribute;
using Onw.Manager.Prototype;
using Onw.Manager.ObjectPool;
using TM.Synergy;

namespace TM.UI
{
    using ListPool = ListPool<KeyValuePair<string, TMSynergyScrollItem>>;
    
    public sealed class TMSynergyViewController : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _scrollItemPrefabReference;
        [SerializeField, SelectableSerializeField] private ScrollRect _scrollView;
        
        private readonly Dictionary<string, TMSynergyScrollItem> _scrollItems = new();

        public void OnUpdateSynergies(TMSynergy[] synergies)
        {
            List<KeyValuePair<string, TMSynergyScrollItem>> removeKeys = ListPool.Get();
                
            removeKeys.AddRange(_scrollItems
                .Where(synergyScrollItemKvp => synergies.All(synergy => synergy.SynergyData.ID != synergyScrollItemKvp.Key)));

            foreach (KeyValuePair<string, TMSynergyScrollItem> synergyScrollItemKvp in removeKeys)
            {
                _scrollItems.Remove(synergyScrollItemKvp.Key);
                GenericObjectPool<TMSynergyScrollItem>.Return(synergyScrollItemKvp.Value);
            }
                
            ListPool.Release(removeKeys);
                
            foreach (TMSynergy synergy in synergies)
            {
                if (!_scrollItems.TryGetValue(synergy.SynergyData.ID, out TMSynergyScrollItem scrollItem))
                {
                    if (!GenericObjectPool<TMSynergyScrollItem>.TryPop(out scrollItem))
                    {
                        scrollItem = PrototypeManager.Instance.ClonePrototypeFromReferenceSync<TMSynergyScrollItem>(_scrollItemPrefabReference);
                    }
                        
                    _scrollItems.Add(synergy.SynergyData.ID, scrollItem);
                }
                    
                scrollItem.transform.SetParent(_scrollView.content, false);
                scrollItem.SetView(synergy);
            }
        }
    }
}