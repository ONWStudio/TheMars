#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace ButtonAttributeEditorSpace
{
    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    internal sealed class InspectorButtonAttributeDrawer : Editor
    {
        private readonly List<KeyValuePair<string, MethodInfo>> _buttonMethods = new();

        private void OnEnable()
        {
            _buttonMethods.AddRange(target
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(method => new KeyValuePair<string, MethodInfo>((method.GetCustomAttribute(typeof(InspectorButtonAttribute)) as InspectorButtonAttribute)?.ButtonName, method))
                .Where(kvp => kvp.Key != null));
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _buttonMethods
                .Where(kvp => GUILayout.Button(kvp.Key))
                .ForEach(kvp => kvp.Value.Invoke(target, null));
        }
    }
}
#endif
