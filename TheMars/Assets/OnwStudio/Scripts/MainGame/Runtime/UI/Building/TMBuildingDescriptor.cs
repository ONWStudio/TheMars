using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using TMPro;

namespace TM.UI
{
    public class TMBuildingDescriptor : MonoBehaviour
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

        public bool ActiveDescriptor
        {
            get => _canvas.enabled;
            set => _canvas.enabled = value;
        }
        
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _nameText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
    }
}