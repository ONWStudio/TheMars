#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Onw.Editor;
using Onw.Helper;
using Onw.ScriptableObjects.Editor;
using TMCard.Runtime;

namespace TMCard.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(TMCardData), false), CanEditMultipleObjects]
    public sealed class TMCardDataSODrawer : Editor
    {
        private const int PREVIEW_CARD_WIDTH = 256;

        private RenderTexture _renderTexture = null;
        private Scene _previewScene;
        private Camera _previewCamera = null;
        private Canvas _previewCanvas = null;
        private TMCardGeneralController _previewInstance = null;

        private void OnEnable()
        {
            if (target is not TMCardData targetObject) return;

            string name = targetObject.CardName.EntryKeyName;
            if (!string.IsNullOrEmpty(name) && name != target.name)
            {
                string path = AssetDatabase.GetAssetPath(targetObject);

                if (ScriptableObjectHandler<TMCardData>.CheckDuplicatedName(path, name))
                {
                    Debug.LogWarning("이미 해당 카드와 같은 이름을 가지고 있는 카드가 있으므로 카드의 이름을 설정 할 수 없습니다");
                }
                else
                {
                    ScriptableObjectHandler<TMCardData>.RenameScriptableObject(targetObject, name);
                }
            }

            string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab");

            foreach (string prefabGUID in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabGUID);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab && prefab.TryGetComponent(out TMCardGeneralController previewInstance))
                {
                    _previewScene = EditorSceneManager.NewPreviewScene();
                    _previewScene.name = "Preview Scene";

                    GameObject canvasGO = new("Preview Canvas");
                    _previewCamera = new GameObject("Preview Camera").AddComponent<Camera>();

                    SceneManager.MoveGameObjectToScene(canvasGO, _previewScene);
                    SceneManager.MoveGameObjectToScene(_previewCamera.gameObject, _previewScene);

                    _previewCamera.clearFlags = CameraClearFlags.SolidColor;
                    _previewCamera.backgroundColor = Color.gray;
                    _previewCamera.orthographic = true;
                    _previewCamera.orthographicSize = 5;
                    _previewCamera.cullingMask = LayerMask.GetMask("UI");
                    _previewCamera.scene = _previewScene;
                    _previewCamera.cameraType = CameraType.Preview;

                    _previewCanvas = canvasGO.AddComponent<Canvas>();
                    _previewCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    _previewCanvas.worldCamera = _previewCamera;
                    _previewCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
                    canvasGO.AddComponent<CanvasScaler>();
                    canvasGO.AddComponent<GraphicRaycaster>();

                    _previewInstance = Instantiate(previewInstance, _previewCanvas.transform, false);
                    TMCardController controller = _previewInstance.GetComponent<TMCardController>();
                    controller.CardData = targetObject;
                    controller.Initialize();
                    _previewInstance.SetCardData(targetObject);

                    _previewCamera.transform.position = _previewInstance.transform.position - Vector3.forward * 10;

                    if (_previewInstance.transform is RectTransform rectTransform)
                    {
                        float ratio = rectTransform.rect.height / rectTransform.rect.width;
                        _renderTexture = new RenderTexture(PREVIEW_CARD_WIDTH, (int)(PREVIEW_CARD_WIDTH * ratio), 16);
                        _previewCamera.targetTexture = _renderTexture;
                    }

                    break;
                }
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
        }

        private void OnDestroy()
        {
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewInstance);
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewCamera);
            OnwUnityHelper.DestroyImmediateObjectByComponent(ref _previewCanvas);
            OnwUnityHelper.ReleaseRenderTexture(ref _renderTexture);
            EditorSceneManager.ClosePreviewScene(_previewScene);
        }
    }
}
#endif