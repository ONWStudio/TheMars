using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [System.Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
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
            serialize();
        }

        private void serialize()
        {
            Clear();

            foreach (SerializedKeyValuePair<TKey, TValue> kvp in _serializedList.Where(kvp => !ContainsKey(kvp.Key)))
            {
                Add(kvp.Key, kvp.Value);
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
            serialize();
        }

        public void NewRemove(TKey key, TValue value)
        {
            _serializedList.Remove(new(key, value));
        }

        public void NewClear()
        {
            Clear();
            _serializedList.Clear();
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
