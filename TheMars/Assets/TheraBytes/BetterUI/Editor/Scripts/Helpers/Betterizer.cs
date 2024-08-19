using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    public static class Betterizer
    {

        public static T MakeBetter<T, TBetter>(T source, params string[] skipFields)
            where T : MonoBehaviour
            where TBetter : T
        {
            string typeName = typeof(T).Name;

    //        if (typeof(TBetter).GetInterfaces().Contains(typeof(IResolutionDependency)))
    //        {
    //            if (ResolutionMonitor.CurrentResolution != ResolutionMonitor.OptimizedResolution)
    //            {
    //                if (!(EditorUtility.DisplayDialog("Not working with Optimized resolution",
    //string.Format("Optimized Resolution: {0} x {1}{5}Current Resolution: {2} x {3}{5}"
    //+ "You should work with optimized resolution "
    //+ "whenever you make a {4} better to avoid unexpected sizes. {5}"
    //+ "Do you want to make it better anyway?",
    //ResolutionMonitor.OptimizedResolution.x, ResolutionMonitor.OptimizedResolution.y,
    //ResolutionMonitor.CurrentResolution.x, ResolutionMonitor.CurrentResolution.y,
    //typeName, Environment.NewLine),
    //"Yes, make it Better", "No, I will change the resolution.")))
    //                {
    //                    return null;
    //                }

    //            }
    //        }

            if (source is TBetter)
            {

                if (EditorUtility.DisplayDialog("Already Good Enough",
                    string.Format("This already is a 'Better {0}'.{1}Do you want to downgrade it to '{0}'?",
                    typeName, Environment.NewLine),
                    "Yes", "No"))
                {
                    return Convert<T, TBetter>(source, true, skipFields);
                }
                return null;
            }

            return Convert<T, TBetter>(source, false, skipFields);
        }

        private static T Convert<T, TBetter>(T source, bool downgrade, params string[] skipFields)
            where T : MonoBehaviour
            where TBetter : T
        {
            KeyValuePair<FieldInfo, object>[] fields = CollectAllFieldValues(typeof(T), source, new HashSet<string>(skipFields)).ToArray();
            HashSet<KeyValuePair<SerializedObject, string>> refs = new HashSet<KeyValuePair<SerializedObject, string>>(FindReferencesTo(source));

            GameObject go = source.gameObject;

            int order = GetComponentOrder(source);

            Undo.SetCurrentGroupName(string.Format("Make Better {0}", DateTime.Now.ToFileTimeUtc()));

            Undo.DestroyObjectImmediate(source);
            T better = (downgrade)
                ? Undo.AddComponent<T>(go)
                : Undo.AddComponent<TBetter>(go);


            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

            foreach (KeyValuePair<FieldInfo, object> kv in fields)
            {
                try
                {
                    kv.Key.SetValue(better, kv.Value);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("Could not set value {0}: {1}", kv.Key.Name, ex.Message);
                }
            }

            foreach (KeyValuePair<SerializedObject, string> r in refs)
            {
                SetReference(r.Key, r.Value, better);
            }


            for (int i = 0; i < order; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(better);
            }

            return better;
        }

        public static void Validate(UnityEngine.EventSystems.UIBehaviour obj)
        {
            if (obj == null)
                return;

            Type t = obj.GetType();
            t.InvokeMember("OnValidate",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                null, obj, null);
        }


        private static int GetComponentOrder(Component comp)
        {
            int idx = 0;
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(comp))
            {
                idx++;
            }
            return idx;
        }


        private static IEnumerable<KeyValuePair<FieldInfo, object>> CollectAllFieldValues(Type type, object source,
            HashSet<string> skipFields)
        {
            foreach (FieldInfo field in CollectFieldInfosRecursively(type))
            {
                // skip private fields which are not marked as SerializeField
                if (!(field.IsPublic) && (field.GetCustomAttributes(typeof(SerializeField), true).Length == 0))
                    continue;

                // skip public fields which are marked as NonSerialized
                if ((field.IsPublic) && (field.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0))
                    continue;

                // skip special fields which make problems copying them
                if (skipFields.Contains(field.Name))
                    continue;

                // skip arrays for now...
                // find a solution to copy it if needed
                if (field.FieldType.IsArray)
                {
                    Debug.LogWarningFormat("Collect Fields: Array '{0}' skipped", field.Name);
                    continue;
                }

                object value = field.GetValue(source);
                value = CreateCopyIfNoUnityObject(value);

                yield return new KeyValuePair<FieldInfo, object>(field, value);
            }
        }

        private static IEnumerable<FieldInfo> CollectFieldInfosRecursively(Type type)
        {
            while (type != typeof(object))
            {
                foreach (FieldInfo field in CollectFieldInfos(type))
                {
                    yield return field;
                }

                type = type.BaseType;
            }
        }

        private static IEnumerable<FieldInfo> CollectFieldInfos(Type type)
        {
            FieldInfo[] myObjectFields = type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in myObjectFields)
            {
                yield return field;
            }
        }

        private static object CreateCopyIfNoUnityObject(object original)
        {
            if (original is UnityEngine.Object || original == null)
            {
                return original;
            }
            else
            {
                Type type = original.GetType();
                object copy = (type == typeof(string))
                    ? original as string
                    : Activator.CreateInstance(type);

                CopyValuesRecursive(type, original, copy);

                return copy;
            }
        }

        public static void CopyValuesRecursive(Type type, object source, object target)
        {

            while (type != typeof(object))
            {
                CopyValues(type, source, target);
                type = type.BaseType;
            }
        }

        public static void CopyValues(Type type, object source, object target)
        {
            FieldInfo[] myObjectFields = type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo fi in myObjectFields)
            {
                fi.SetValue(target, fi.GetValue(source));
            }
        }


        private static IEnumerable<KeyValuePair<SerializedObject, string>> FindReferencesTo(UnityEngine.Component obj)
        {
            // iterate objects in the scene
            GameObject[] allObjects =
#if UNITY_2022_2_OR_NEWER
                UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
                UnityEngine.Object.FindObjectsOfType<GameObject>();
#endif

            for (int i = 0; i < allObjects.Length; i++)
            {
                GameObject go = allObjects[i];
                foreach (KeyValuePair<SerializedObject, string> keyValuePair in FindReferencesTo(obj, go))
                {
                    yield return keyValuePair;
                }
            }

            // iterate object in this prefab
            foreach (Transform transform in IterateChildrenRecursively(obj.transform.root))
            {
                GameObject go = transform.gameObject;
                foreach (KeyValuePair<SerializedObject, string> keyValuePair in FindReferencesTo(obj, go))
                {
                    yield return keyValuePair;
                }
            }
        }

        private static IEnumerable<KeyValuePair<SerializedObject, string>> FindReferencesTo(Component obj, GameObject go)
        {
            Component[] components = go.GetComponents<Component>();
            for (int k = 0; k < components.Length; k++)
            {
                Component comp = components[k];
                if (comp == null || comp == obj)
                    continue;

                SerializedObject so = new SerializedObject(comp);
                SerializedProperty sp = so.GetIterator();
                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference
                        && sp.objectReferenceValue == obj)
                    {
                        string path = sp.propertyPath;
                        yield return new KeyValuePair<SerializedObject, string>(so, path);
                    }
                }
            }
        }

        private static void SetReference(SerializedObject so, string path, UnityEngine.Object obj)
        {
            SerializedProperty prop = so.FindProperty(path);

            //Debug.LogWarningFormat("{0} ({1})", path, prop.propertyType);

            prop.objectReferenceValue = obj;
            so.ApplyModifiedProperties();
        }

        private static IEnumerable<Transform> IterateChildrenRecursively(Transform self)
        {
            yield return self;
            
            for (int i = 0; i < self.childCount; i++)
            {
                Transform child = self.GetChild(i);

                foreach (Transform c in IterateChildrenRecursively(child))
                {
                    yield return c;
                }
            }
        }


    }
}
