using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using TM.Tutorial;
using Onw.Extensions;
using System.Linq;

namespace TM.UI
{
    public class TMTutorialController : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private Canvas _canvas;
        [SerializeField, InitializeRequireComponent] private AudioSource _audioSource;
        [SerializeField, SelectableSerializeField] private Button _button;
        [SerializeField, SelectableSerializeField] private Canvas _satisfactionCanvas;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private List<TMTutorialData> _tutorialDataList;
        private TMTutorialData _currentTutorial = null;

        private void Awake()
        {
            SetActiveTutorialUI(true);
        }

        public void SetActiveTutorialUI(bool isActive)
        {
            _canvas.enabled = isActive;

            if (!isActive && _satisfactionCanvas.enabled)
            {
                _satisfactionCanvas.enabled = isActive;
            }

            if (isActive && _tutorialDataList.Count > 0)
            {
                if (_currentTutorial)
                {
                    _currentTutorial.LocalizedDescription.StringChanged -= setDescription;
                }

                _currentTutorial = _tutorialDataList.Pop(_tutorialDataList.Count - 1);
                _satisfactionCanvas.enabled = _currentTutorial.Properties.Any(property => property == "SatisfactionDescription");
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }

                if (_currentTutorial.AudioClip)
                {
                    _audioSource.clip = _currentTutorial.AudioClip;
                    _audioSource.Play();
                }

                _currentTutorial.LocalizedDescription.StringChanged += setDescription;
            }
        }

        public void OnClickButton()
        {
            SetActiveTutorialUI(_tutorialDataList.Count > 0);
        }

        private void setDescription(string description)
        {
            _descriptionText.text = description;
        }
    }
}
