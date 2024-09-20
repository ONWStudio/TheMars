using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Onw.Attribute;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace TM.MainMenu
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private Button _startButton;
        [SerializeField, SelectableSerializeField] private Button _loadButton;
        [SerializeField, SelectableSerializeField] private Button _optionButton;
        [SerializeField, SelectableSerializeField] private Button _collectionButton;
        [SerializeField, SelectableSerializeField] private Button _gameExitButton;

        private void Start()
        {
            _startButton.onClick.AddListener(onClickStartButton);
            _loadButton.onClick.AddListener(onClickLoadButton);
            _optionButton.onClick.AddListener(onClickOptionButton);
            _collectionButton.onClick.AddListener(onClickCollectionButton);
            _gameExitButton.onClick.AddListener(onClickGameExitButton);

            static void onClickStartButton() => SceneManager.LoadScene("MainGameScene");
            static void onClickLoadButton() { }
            static void onClickOptionButton() { }
            static void onClickCollectionButton() { }

            static void onClickGameExitButton()
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}