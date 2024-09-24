using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Onw.Attribute;
using Onw.Manager.ObjectPool;
using TM.Synergy;

namespace TM.Runtime.UI
{
    public sealed class SynergyViewController : MonoBehaviour
    {
        [SerializeField] private TMSynergyScrollItem _scrollItemPrefab;
        [SerializeField, InitializeRequireComponent] private ScrollRect _scrollView;
        
        private readonly Dictionary<string, TMSynergyScrollItem> _scrollItems = new();
        
        private void Start()
        {
            TMSynergyManager.Instance.OnUpdateSynergies += onUpdateSynergies;

            void onUpdateSynergies(IReadOnlyDictionary<string, TMSynergy> dictionary)
            {
                List<KeyValuePair<string, TMSynergyScrollItem>> removeKeys = ListPool<KeyValuePair<string, TMSynergyScrollItem>>.Get();
                
                removeKeys.AddRange(_scrollItems
                    .Where(synergyScrollItemKvp => !dictionary.ContainsKey(synergyScrollItemKvp.Key)));

                foreach (KeyValuePair<string, TMSynergyScrollItem> synergyScrollItemKvp in removeKeys)
                {
                    _scrollItems.Remove(synergyScrollItemKvp.Key);
                    GenericObjectPool<TMSynergyScrollItem>.Return(synergyScrollItemKvp.Value);
                }
                
                foreach (KeyValuePair<string, TMSynergy> synergyKvp in dictionary)
                {
                    if (!_scrollItems.TryGetValue(synergyKvp.Key, out TMSynergyScrollItem scrollItem))
                    {
                        if (!GenericObjectPool<TMSynergyScrollItem>.TryPop(out scrollItem))
                        {
                            scrollItem = Instantiate(_scrollItemPrefab.gameObject).GetComponent<TMSynergyScrollItem>();
                        }
                        
                        _scrollItems.Add(synergyKvp.Key, scrollItem);
                    }
                    
                    scrollItem.transform.SetParent(_scrollView.content, false);
                    scrollItem.SetView(synergyKvp.Value);
                }
            }
        }
    }
}