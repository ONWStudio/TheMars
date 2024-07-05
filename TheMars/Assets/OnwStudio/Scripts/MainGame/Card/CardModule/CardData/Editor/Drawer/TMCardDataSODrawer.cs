#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Search;
using Onw.Editor;
using TMCard.UI;

namespace TMCard.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(TMCardData), false), CanEditMultipleObjects]
    public sealed class TMCardDataSODrawer : Editor
    {
        private Texture2D _renderTexture = null;
        private TMCardUIController _previewInstance = null;
        private TMCardData _cardData = null;

        private void OnEnable()
        {
            Debug.Log("TMCardDataSODrawer Enable");

            if (target is TMCardData targetObject)
            {
                _cardData = targetObject;

                string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab");

                foreach (string prefabGUID in prefabGUIDs)
                {
                    string path = AssetDatabase.GUIDToAssetPath(prefabGUID);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (prefab && prefab.TryGetComponent(out TMCardUIController previewInstance))
                    {
                        _previewInstance = Instantiate(previewInstance);

                        break;
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (_previewInstance)
            {
                DestroyImmediate(_previewInstance.gameObject);
                _previewInstance = null;
            }

            if (_renderTexture)
            {
                DestroyImmediate(_renderTexture);
                _renderTexture = null;
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Debug.Log("TMCardDataSODrawer");

            if (!_renderTexture && _previewInstance)
            {
                _renderTexture = AssetPreview.GetAssetPreview(_previewInstance.gameObject);
            }

            if (_previewInstance && _renderTexture)
            {
                GUILayout.Label("Loaded Card Prefab Preview:");
                EditorGUI.DrawPreviewTexture(
                    GUILayoutUtility.GetRect(256, 256, GUILayout.ExpandWidth(false)),
                    _renderTexture);
            }
        }
    }
}
#endif