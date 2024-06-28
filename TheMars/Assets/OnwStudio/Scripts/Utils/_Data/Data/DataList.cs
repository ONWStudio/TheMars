using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Onw.Data
{
    [Serializable]
    public sealed class JsonList<T>
    {
        [JsonProperty] internal List<T> DataList { get; set; }

        public IReadOnlyList<T> Data => DataList;
        internal JsonList() {}
    }
}
