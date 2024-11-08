using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Manager;
using Onw.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TM.Card
{
    public sealed class TMCardDataManager : ScriptableObjectSingleton<TMCardDataManager>
    {
        private const string FILE_PATH = "OnwStudio/ScriptableObject/ScriptableObjectManager/Resources/TMCardDataManager.asset";

        public IReadOnlyList<TMCardData> CardDataList => _cardDataList;
        
        [SerializeField, ReadOnly] private List<TMCardData> _cardDataList = new();

        #if UNITY_EDITOR
        internal void AddCard(TMCardData cardData)
        {
            if (_cardDataList.Contains(cardData)) return;
            
            _cardDataList.Add(cardData);
            EditorUtility.SetDirty(this);
        }

        internal bool RemoveCard(TMCardData cardData)
        {
            bool isRemove = _cardDataList.Remove(cardData);
            EditorUtility.SetDirty(this);
            
            return isRemove;
        }
        #endif

        public TMCardData[] GetCardDataArrayByWhere(Func<TMCardData, bool> predicate)
        {
            return _cardDataList
                .Where(predicate)
                .ToArray();
        }
    }
}
