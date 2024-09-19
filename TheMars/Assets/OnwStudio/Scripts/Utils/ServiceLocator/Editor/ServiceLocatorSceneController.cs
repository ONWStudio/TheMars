// #if UNITY_EDITOR
// using System.Linq;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using Onw.Extensions;
//
// namespace Onw.ServiceLocator.Editor
// {
//     internal sealed class ServiceLocatorSceneController : EditorWindow
//     {
//         private readonly List<SceneAsset> _scenes = new();
//         private GameObject[] _gameObjectsInScene = null;
//
//         private SceneAsset _selectedScene;
//
//         [MenuItem("Onw Studio/Service Locator/Scene Controller")]
//         private static void showWindow()
//         {
//             GetWindow<ServiceLocatorSceneController>("Service Locator Scene Controller")
//                 .Show();
//         }
//
//         private void OnEnable()
//         {
//             _scenes.AddRange(
//                 AssetDatabase
//                     .FindAssets("t:Scene")
//                     .Select(guid => AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guid))));
//
//             _gameObjectsInScene = SceneManager
//                 .GetActiveScene()
//                 .GetRootGameObjects()
//                 .SelectMany(gameObject => gameObject.GetChildGameObjectsAll()).ToArray();
//         }
//
//         private void OnGUI()
//         {
//             string scenePath = AssetDatabase.GetAssetPath(_selectedScene);
//             EditorSceneManager.OpenScene(scenePath);
//         }
//     }
// }
// #endif