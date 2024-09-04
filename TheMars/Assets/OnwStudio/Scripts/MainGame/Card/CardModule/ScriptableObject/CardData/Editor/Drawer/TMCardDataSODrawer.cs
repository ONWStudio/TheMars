#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines;
using Onw.Helper;
using Onw.Localization;
using Onw.ScriptableObjects.Editor;
using TMCard.Runtime;
using Unity.EditorCoroutines.Editor;

namespace TMCard.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(TMCardData), false), CanEditMultipleObjects]
    public sealed class TMCardDataSoDrawer : Editor
    {
        private const int PREVIEW_CARD_WIDTH = 256;

        private RenderTexture _renderTexture = null;
        private Scene? _previewScene = null;
        private Camera _previewCamera = null;
        private Canvas _previewCanvas = null;
        private TMCardController _previewInstance = null;
        private bool _isUpdate = false;

        private void OnEnable()
        {
            if (target is not TMCardData targetObject) return;
            
            if (targetObject.GetType().GetField("_localizedCardName")?.GetValue(targetObject) is LocalizedString localizedString)
            {
                string entryKeyName = localizedString.GetEntryKeyName();
                
                if (!string.IsNullOrEmpty(entryKeyName) && entryKeyName != target.name)
                {
                    string path = AssetDatabase.GetAssetPath(targetObject);

                    if (ScriptableObjectHandler<TMCardData>.CheckDuplicatedName(path, entryKeyName))
                    {
                        Debug.LogWarning("이미 해당 카드와 같은 이름을 가지고 있는 카드가 있으므로 카드의 이름을 설정 할 수 없습니다");
                    }
                    else
                    {
                        ScriptableObjectHandler<TMCardData>.RenameScriptableObject(targetObject, entryKeyName);
                    }
                }
            }
            
            createPreviewObject(); 
            
            if (targetObject.GetType().GetField("_onValueChanged", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(targetObject) is SafeAction safeAction)
            {
                safeAction.AddListener(onChangedValue);
            }
        }

        private void onChangedValue()
        {
            _isUpdate = true;
        }
        
        private void OnDisable()
        {
            TMCardData targetObject = target as TMCardData;
            
            if (targetObject.GetType().GetField("_onValueChanged", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(targetObject) is SafeAction safeAction)
            {
                // ReSharper disable once RedundantAssignment
                safeAction.RemoveListener(onChangedValue);
            }
        }

        public override void OnInspectorGUI()
        {
            if (_previewInstance && _renderTexture)
            {
                GUILayout.Label("Card Preview");

                EditorGUI.DrawPreviewTexture(
                    GUILayoutUtility.GetRect(
                        _renderTexture.width,
                        _renderTexture.height,
                        GUILayout.ExpandWidth(false),
                        GUILayout.ExpandHeight(false)),
                    _renderTexture,
                    null,
                    ScaleMode.ScaleToFit);
            }

            EditorGUILayout.Space(10);
            DrawDefaultInspector();

            if (_isUpdate)
            {
                _isUpdate = false;
                destroyPreviewObject();
                createPreviewObject();
            }
        }

        private void createPreviewObject()
        {
            TMCardData targetObject = target as TMCardData;

            string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab");

            foreach (string prefabGuid in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab && prefab.TryGetComponent(out TMCardController previewInstance))
                {
                    _previewScene = EditorSceneManager.NewPreviewScene();

                    GameObject canvasGo = new("Preview Canvas");
                    _previewCamera = new GameObject("Preview Camera").AddComponent<Camera>();

                    SceneManager.MoveGameObjectToScene(canvasGo, (Scene)_previewScene);
                    SceneManager.MoveGameObjectToScene(_previewCamera.gameObject, (Scene)_previewScene);

                    _previewCamera.clearFlags = CameraClearFlags.SolidColor;
                    _previewCamera.backgroundColor = Color.gray;
                    _previewCamera.orthographic = true;
                    _previewCamera.orthographicSize = 5;
                    _previewCamera.cullingMask = LayerMask.GetMask("UI");
                    _previewCamera.scene = (Scene)_previewScene;
                    _previewCamera.cameraType = CameraType.Preview;

                    _previewCanvas = canvasGo.AddComponent<Canvas>();
                    _previewCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    _previewCanvas.worldCamera = _previewCamera;
                    _previewCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
                    canvasGo.AddComponent<CanvasScaler>();
                    canvasGo.AddComponent<GraphicRaycaster>();

                    _previewInstance = Instantiate(previewInstance, _previewCanvas.transform, false);
                    TMCardModel cardModel = _previewInstance.GetComponent<TMCardModel>();
                    cardModel.CardData = targetObject;
                    cardModel.Initialize();
                    _previewInstance.CardData = targetObject;

                    _previewCamera.transform.position = _previewInstance.transform.position - Vector3.forward * 10;

                    if (_previewInstance.transform is RectTransform rectTransform)
                    {
                        float ratio = rectTransform.rect.height / rectTransform.rect.width;
                        _renderTexture = new(PREVIEW_CARD_WIDTH, (int)(PREVIEW_CARD_WIDTH * ratio), 16);
                        _previewCamera.targetTexture = _renderTexture;
                    }

                    break;
                }
            }
        }

        private void destroyPreviewObject()
        {
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewInstance);
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewCamera);
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewCanvas);
            OnwUnityHelper.ReleaseRenderTexture(ref _renderTexture);
            if (_previewScene != null)
            {
                EditorSceneManager.ClosePreviewScene((Scene)_previewScene);
            }
            _previewScene = null;
        }

        private void OnDestroy()
        {
            destroyPreviewObject();
        }
    }
}
#endif