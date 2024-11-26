using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Onw.Attribute;
using UnityEngine.Serialization;

namespace TM.UI
{
    public class TMSynergyDescriptor : MonoBehaviour
    {
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
        
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _nameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;

        public void SetActiveDescriptor(bool isActive)
        {
            _canvas.enabled = isActive;
        }
    }
}
