using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using Onw.Manager.Prototype;
using TM.Synergy;
using UnityEngine.AddressableAssets;

namespace TM.Runtime.UI
{
    using ListPool = ListPool<KeyValuePair<string, TMSynergyScrollItem>>;
    
    public sealed class TMSynergyViewController : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _scrollItemPrefabReference;
        [SerializeField, InitializeRequireComponent] private ScrollRect _scrollView;
        
        private readonly Dictionary<string, TMSynergyScrollItem> _scrollItems = new();
        
        private void Start()
        {
            TMSynergyManager.Instance.OnUpdateSynergies += onUpdateSynergies;

            void onUpdateSynergies(TMSynergy[] synergies)
            {
                List<KeyValuePair<string, TMSynergyScrollItem>> removeKeys = ListPool.Get();
                
                removeKeys.AddRange(_scrollItems
                    .Where(synergyScrollItemKvp => synergies.All(synergy => synergy.SynergyName != synergyScrollItemKvp.Key)));

                foreach (KeyValuePair<string, TMSynergyScrollItem> synergyScrollItemKvp in removeKeys)
                {
                    _scrollItems.Remove(synergyScrollItemKvp.Key);
                    GenericObjectPool<TMSynergyScrollItem>.Return(synergyScrollItemKvp.Value);
                }
                
                ListPool.Release(removeKeys);
                
                foreach (TMSynergy synergy in synergies)
                {
                    if (!_scrollItems.TryGetValue(synergy.SynergyName, out TMSynergyScrollItem scrollItem))
                    {
                        if (!GenericObjectPool<TMSynergyScrollItem>.TryPop(out scrollItem))
                        {
                            scrollItem = PrototypeManager.Instance.ClonePrototypeFromReferenceSync<TMSynergyScrollItem>(_scrollItemPrefabReference);
                        }
                        
                        _scrollItems.Add(synergy.SynergyName, scrollItem);
                    }
                    
                    scrollItem.transform.SetParent(_scrollView.content, false);
                    scrollItem.SetView(synergy);
                }
            }
        }
    }
}