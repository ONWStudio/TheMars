using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable 0618 // disable "never assigned" warnings

namespace TheraBytes.BetterUi
{
    [HelpURL("https://documentation.therabytes.de/better-ui/ResolutionMonitor.html")]
    public class ResolutionMonitor : SingletonScriptableObject<ResolutionMonitor>
    {
        private static string FilePath { get { return "TheraBytes/Resources/ResolutionMonitor"; } }


        #region Obsolete
        [Obsolete("Use 'GetOptimizedResolution()' instead.")]
        public static Vector2 OptimizedResolution
        {
            get { return ResolutionMonitor.Instance.optimizedResolutionFallback; }
            set
            {
                if (ResolutionMonitor.Instance.optimizedResolutionFallback == value)
                    return;

                ResolutionMonitor.Instance.optimizedResolutionFallback = value;
                CallResolutionChanged();
            }
        }

        [Obsolete("Use 'GetOptimizedDpi()' instead.")]
        public static float OptimizedDpi
        {
            get { return ResolutionMonitor.Instance.optimizedDpiFallback; }
            set
            {
                if (ResolutionMonitor.Instance.optimizedDpiFallback == value)
                    return;

                ResolutionMonitor.Instance.optimizedDpiFallback = value;
                CallResolutionChanged();
            }
        }
        #endregion

        public static Vector2 CurrentResolution
        {
            get
            {
                if (lastScreenResolution == Vector2.zero)
                {
                    lastScreenResolution = new Vector2(Screen.width, Screen.height);
                }

                return lastScreenResolution;
            }
        }

        public static float CurrentDpi
        {
            get
            {
                if (lastDpi == 0)
                {
                    lastDpi = Instance.dpiManager.GetDpi();
                }

                return lastDpi;
            }
        }

        public string FallbackName { get { return fallbackName; } set { fallbackName = value; } }

        public static Vector2 OptimizedResolutionFallback { get { return ResolutionMonitor.Instance.optimizedResolutionFallback; } }
        public static float OptimizedDpiFallback { get { return ResolutionMonitor.Instance.optimizedDpiFallback; } }

        [FormerlySerializedAs("optimizedResolution")]
        [SerializeField]
        private Vector2 optimizedResolutionFallback = new Vector2(1080, 1920);

        [FormerlySerializedAs("optimizedDpi")]
        [SerializeField]
        private float optimizedDpiFallback = 96;

        [SerializeField]
        private string fallbackName = "Portrait";

        [SerializeField]
        private StaticSizerMethod[] staticSizerMethods = new StaticSizerMethod[5];

        [SerializeField]
        private DpiManager dpiManager = new DpiManager();

        private ScreenTypeConditions currentScreenConfig;

        [SerializeField]
        private List<ScreenTypeConditions> optimizedScreens = new List<ScreenTypeConditions>()
        {
            new ScreenTypeConditions("Landscape", typeof(IsCertainScreenOrientation)),
        };

        public List<ScreenTypeConditions> OptimizedScreens { get { return optimizedScreens; } }

        private static Dictionary<string, ScreenTypeConditions> lookUpScreens = new Dictionary<string, ScreenTypeConditions>();

        #region Screen Tags

        private static HashSet<string> screenTags = new HashSet<string>();
        public static IEnumerable<string> CurrentScreenTags { get { return screenTags; } }

        public static bool AddScreenTag(string screenTag)
        {
            if (screenTags.Add(screenTag))
            {
                isDirty = true;
                Update();
                return true;
            }

            return false;
        }

        public static bool RemoveScreenTag(string screenTag)
        {
            if (screenTags.Remove(screenTag))
            {
                isDirty = true;
                Update();
                return true;
            }

            return false;
        }

        public static void ClearScreenTags()
        {
            screenTags.Clear();
            isDirty = true;
            Update();
        }

        #endregion

#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
        private static UnityEditor.SceneManagement.StageHandle currentStage;
#endif

        public static ScreenTypeConditions CurrentScreenConfiguration
        {
            get
            {
#if UNITY_EDITOR
                if (simulatedScreenConfig != null)
                {
                    return simulatedScreenConfig;
                }
#endif
                return ResolutionMonitor.Instance.currentScreenConfig;
            }
        }

        public static ScreenTypeConditions GetConfig(string name)
        {
            if (lookUpScreens.Count == 0)
            {
                foreach (ScreenTypeConditions config in ResolutionMonitor.Instance.optimizedScreens)
                {
                    lookUpScreens.Add(config.Name, config);
                }
            }

            if (!(lookUpScreens.ContainsKey(name)))
            {
                ScreenTypeConditions config = ResolutionMonitor.Instance.optimizedScreens.FirstOrDefault(o => o.Name == name);

                if (config != null)
                {
                    lookUpScreens.Add(name, config);
                    return config;
                }
                else
                {
                    return null;
                }
            }

            return lookUpScreens[name];
        }


        public static ScreenInfo GetOpimizedScreenInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new ScreenInfo(OptimizedResolutionFallback, OptimizedDpiFallback);
            }

            return GetConfig(name).OptimizedScreenInfo;
        }


        public static IEnumerable<ScreenTypeConditions> GetCurrentScreenConfigurations()
        {
            foreach (ScreenTypeConditions config in ResolutionMonitor.Instance.optimizedScreens)
            {
                if (config.IsActive)
                    yield return config;
            }
        }


        private static Vector2 lastScreenResolution;
        private static float lastDpi;

        private static bool isDirty;

#if UNITY_EDITOR
        private static Type gameViewType = null;
        private static UnityEditor.EditorWindow gameViewWindow = null;
        private static Version unityVersion;

        private static ScreenTypeConditions simulatedScreenConfig;
        public static ScreenTypeConditions SimulatedScreenConfig
        {
            get { return simulatedScreenConfig; }
            set
            {
                if (simulatedScreenConfig != value)
                    isDirty = true;

                simulatedScreenConfig = value;
            }
        }

        private void OnEnable()
        {
            RegisterCallbacks();
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private static void RegisterCallbacks()
        {
            unityVersion = UnityEditorInternal.InternalEditorUtility.GetUnityVersion();

            isDirty = true;

            UnityEditor.EditorApplication.update += Update;

#if UNITY_5_6_OR_NEWER
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += SceneOpened;
#endif

#if UNITY_2018_0_OR_NEWER
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChanged;
#else
            UnityEditor.EditorApplication.playmodeStateChanged += PlayModeStateChanged;
#endif
        }

        private static void UnregisterCallbacks()
        {
            UnityEditor.EditorApplication.update -= Update;

#if UNITY_5_6_OR_NEWER
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened -= SceneOpened;
#endif

#if UNITY_2018_0_OR_NEWER
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChanged;
#else
            UnityEditor.EditorApplication.playmodeStateChanged -= PlayModeStateChanged;
#endif
        }


#if UNITY_5_6_OR_NEWER
        private static void SceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            isDirty = true;
            Update();
        }
#endif


        private static void PlayModeStateChanged()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                ClearScreenTags();
            }

            Instance.ResolutionChanged();
        }
#else
        void OnEnable()
        {
            ResolutionChanged();
        }
#endif

        public static float InvokeStaticMethod(ImpactMode mode, Component caller, Vector2 optimizedResolution, Vector2 actualResolution, float optimizedDpi, float actualDpi)
        {
            int idx = mode switch
            {
                ImpactMode.StaticMethod1 => 0,
                ImpactMode.StaticMethod2 => 1,
                ImpactMode.StaticMethod3 => 2,
                ImpactMode.StaticMethod4 => 3,
                ImpactMode.StaticMethod5 => 4,
                _ => throw new ArgumentException(),
            };
            return (ResolutionMonitor.HasInstance && Instance.staticSizerMethods[idx] != null)
                ? Instance.staticSizerMethods[idx].Invoke(caller, optimizedResolution, actualResolution, optimizedDpi, actualDpi)
                : 1;
        }


        public static void MarkDirty()
        {
            ResolutionMonitor.isDirty = true;
        }

        public static float GetOptimizedDpi(string screenName)
        {
            if (string.IsNullOrEmpty(screenName) || screenName == Instance.fallbackName)
                return OptimizedDpiFallback;

            ScreenTypeConditions s = Instance.optimizedScreens.FirstOrDefault(o => o.Name == screenName);
            if (s == null)
            {
                Debug.LogError("Screen Config with name " + screenName + " could not be found.");
                return OptimizedDpiFallback;
            }

            return s.OptimizedDpi;
        }


        public static Vector2 GetOptimizedResolution(string screenName)
        {
            if (string.IsNullOrEmpty(screenName) || screenName == Instance.fallbackName)
                return OptimizedResolutionFallback;

            ScreenTypeConditions s = GetConfig(screenName);
            if (s == null)
                return OptimizedResolutionFallback;

            return s.OptimizedResolution;
        }

        public static bool IsOptimizedResolution(int width, int height)
        {
            if ((int)OptimizedResolutionFallback.x == width && (int)OptimizedResolutionFallback.y == height)
                return true;

            foreach (ScreenTypeConditions config in Instance.optimizedScreens)
            {
                ScreenInfo si = config.OptimizedScreenInfo;
                if (si != null && (int)si.Resolution.x == width && (int)si.Resolution.y == height)
                    return true;
            }

            return false;
        }

        public static void Update()
        {
#if UNITY_EDITOR
            // check if file was deleted
            if (!HasInstance)
            {
                UnregisterCallbacks();
                return;
            }
#endif

            isDirty = isDirty
#if UNITY_EDITOR // should never change in reality...
                || (Instance.GetCurrentDpi() != lastDpi)
#endif
                || GetCurrentResolution() != lastScreenResolution;

#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
            if (!isDirty)
            {
                StageHandle stage = UnityEditor.SceneManagement.StageUtility.GetCurrentStageHandle();
                if (stage != currentStage)
                {
                    currentStage = stage;
                    isDirty = true;
                }
            }
#endif

            if (isDirty)
            {
                CallResolutionChanged();
                isDirty = false;
            }
        }

        public static void CallResolutionChanged()
        {
            Instance.ResolutionChanged();
        }

        public void ResolutionChanged()
        {
            lastScreenResolution = GetCurrentResolution();
            lastDpi = GetCurrentDpi();

            currentScreenConfig = null;

            bool foundConfig = false;
            foreach (ScreenTypeConditions config in optimizedScreens)
            {
                if (config.IsScreenType() && !(foundConfig))
                {
                    currentScreenConfig = config;
                    foundConfig = true;
                }
            }

            if (ResolutionMonitor.HasInstance) // preserve calling too early
            {
                foreach (IResolutionDependency rd in AllResolutionDependencies())
                {
                    if (!(rd as Behaviour).isActiveAndEnabled)
                        continue;

                    rd.OnResolutionChanged();
                }
            }

#if UNITY_EDITOR
            if (IsZoomPossible())
            {
                FindAndStoreGameView();
                if (gameViewWindow != null)
                {
                    MethodInfo method = gameViewType.GetMethod("UpdateZoomAreaAndParent",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                    try
                    {
                        method?.Invoke(gameViewWindow, null);
                    }
                    catch (Exception) { }
                }
            }
#endif
        }

        private static IEnumerable<IResolutionDependency> AllResolutionDependencies()
        {
            IEnumerable<GameObject> allObjects = GetAllEditableObjects();

            // first update the "override screen properties", because other objects rely on them
            foreach (GameObject go in allObjects)
            {
                OverrideScreenProperties[] resDeps = go.GetComponents<OverrideScreenProperties>();
                foreach (IResolutionDependency comp in resDeps)
                {
                    yield return comp;
                }
            }

            // then update all other objects
            foreach (GameObject go in allObjects)
            {
                IEnumerable<IResolutionDependency> resDeps = go.GetComponents<Behaviour>().OfType<IResolutionDependency>();
                foreach (IResolutionDependency comp in resDeps)
                {
                    if (comp is OverrideScreenProperties)
                        continue;

                    yield return comp;
                }
            }
        }

        private static IEnumerable<GameObject> GetAllEditableObjects()
        {
            GameObject[] allObjects =
#if UNITY_2022_2_OR_NEWER
                UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
                UnityEngine.Object.FindObjectsOfType<GameObject>();
#endif

            foreach (GameObject go in allObjects)
                yield return go;

#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
            PrefabStage prefabStage =
#if UNITY_2021_2_OR_NEWER
                UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
#else
                UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
#endif

            if (prefabStage != null)
            {
                foreach (GameObject root in prefabStage.scene.GetRootGameObjects())
                {
                    foreach (GameObject go in IterateHierarchy(root))
                    {
                        yield return go;
                    }
                }
            }
#endif
        }

        private static IEnumerable<GameObject> IterateHierarchy(GameObject root)
        {
            yield return root;

            foreach (Transform child in root.transform)
            {
                foreach (GameObject subChild in IterateHierarchy(child.gameObject))
                {
                    yield return subChild;
                }
            }
        }

        private static Vector2 GetCurrentResolution()
        {
#if UNITY_EDITOR
            FindAndStoreGameView();

            System.Reflection.MethodInfo GetSizeOfMainGameView = gameViewType.GetMethod("GetSizeOfMainGameView",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            object res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)res;
#else
            return new Vector2(Screen.width, Screen.height);
#endif
        }

        private float GetCurrentDpi()
        {
#if UNITY_EDITOR

            if (IsZoomPossible())
            {
                Vector2 scale = Vector2.one;

                FindAndStoreGameView();

                if (gameViewWindow != null)
                {
                    object zoomArea = gameViewType.GetField("m_ZoomArea",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .GetValue(gameViewWindow);

                    scale = (Vector2)zoomArea.GetType().GetField("m_Scale",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .GetValue(zoomArea);
                }

                return Screen.dpi / scale.y;
            }
#endif
            return dpiManager.GetDpi();
        }

#if UNITY_EDITOR
        private static void FindAndStoreGameView()
        {
            if (gameViewType == null)
            {
                gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
            }

            if (gameViewWindow == null)
            {
                gameViewWindow = Resources.FindObjectsOfTypeAll(gameViewType)
                    .FirstOrDefault() as UnityEditor.EditorWindow;
            }
        }

        public static bool IsZoomPossible()
        {
#if UNITY_2018_3_OR_NEWER // minimum officially supported version
            return true;
#else
            return unityVersion.Major > 5
                || (unityVersion.Major == 5 && unityVersion.Minor >= 4);
#endif
        }

        public void SetOptimizedResolutionFallback(Vector2 resolution)
        {
            this.optimizedResolutionFallback = resolution;
        }
#endif
    }
}

#pragma warning restore 0618
