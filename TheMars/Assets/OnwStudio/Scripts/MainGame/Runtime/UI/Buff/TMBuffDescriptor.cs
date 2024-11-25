using System.Collections;
using System.Collections.Generic;
using Onw.Attribute;
using UnityEngine;
using TMPro;

namespace TM.UI
{
    public class TMBuffDescriptor : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;        
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _timeText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;

        public void SetTimeText(string time)
        {
            _timeText.text = time;
        }

        public void SetDescriptionText(string description)
        {
            _descriptionText.text = description;
        }

        public void SetActiveDescriptor(bool active)
        {
            _canvas.enabled = active;
        }
    }
}