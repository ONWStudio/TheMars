using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Onw.Attribute;

namespace Onw.Localization
{
    [System.Serializable]
    public sealed class LocalizationLoader
    {
        private static bool _isChanged = false;

        [SerializeField, ReadOnly] private LocalizedStringTable _localizedStringTable;

        public LocalizationLoader(string tableName)
        {
            _localizedStringTable = new(tableName);
        }
    }
}
