using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Onw.UI.Components
{
    [DisallowMultipleComponent]
    public sealed class CloseUI : MonoBehaviour
    {
        [SerializeField]
        private BaseUI closeUIObject;

        private Button _closeButton;

        private void Awake()
        {
            _closeButton = GetComponent<Button>();
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() => UIManager.Instance.CloseUI(closeUIObject));
        }
    }
}