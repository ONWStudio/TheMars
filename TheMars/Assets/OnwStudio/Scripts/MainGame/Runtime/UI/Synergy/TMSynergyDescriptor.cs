using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Onw.Attribute;
using UnityEngine.UI;

namespace TM.UI
{
    public class TMSynergyDescriptor : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _nameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
        
        public string Name
        {
            get => _nameText.text;
            set => _nameText.text = value;
        }

        public string Description
        {
            get => _descriptionText.text;
            set => _descriptionText.text = value;
        }

        [field: SerializeField, InitializeRequireComponent] public RectTransform RectTransform { get; private set; }

        public void SetActiveDescriptor(bool isActive)
        {
            _canvas.enabled = isActive;
        }
    }
}
