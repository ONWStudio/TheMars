using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [System.Serializable]
    public partial class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        internal IKeyable LookupTable
        {
            get
            {
                _lookupTable ??= new DictionaryLookupTable<TKey, TValue>(this);
                return _lookupTable;
            }
        }

        private DictionaryLookupTable<TKey, TValue> _lookupTable;
#endif

        [SerializeField]
        internal List<SerializedKeyValuePair<TKey, TValue>> _serializedList = new();

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var kvp in _serializedList)
            {
#if UNITY_EDITOR
                if (!ContainsKey(kvp.Key))
                    Add(kvp.Key, kvp.Value);
#else
                    Add(kvp.Key, kvp.Value);
#endif
            }

#if UNITY_EDITOR
            LookupTable.RecalculateOccurences();
#else
            _serializedList.Clear();
#endif
        }

        /// <summary>
        /// .. Script Only Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void NewAdd(TKey key, TValue value)
        {
            _serializedList.Add(new(key, value));
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                LookupTable.RemoveDuplicates();
#endif
        }
    }
}
