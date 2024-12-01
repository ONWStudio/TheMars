using Onw.Attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace TM.Tutorial
{
    [CreateAssetMenu(fileName = "TMTutorialData", menuName = "Scriptable Object/TutorialData")]
    public class TMTutorialData : ScriptableObject
    {
        [SerializeField] private List<string> _property = new();

        [field: SerializeField, ReadOnly] public string ID { get; private set; } = Guid.NewGuid().ToString();
        [field: SerializeField, LocalizedString(tableName: "TM_Tutorial", entryKey: null)] public LocalizedString LocalizedDescription { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }

        public IReadOnlyList<string> Properties => _property;
    }
}