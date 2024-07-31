#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Onw.Attribute;
using Onw.Helper;
using Onw.Extensions;

namespace Onw.Editor.Attribute
{
    using Editor = UnityEditor.Editor;

    internal sealed class InspectorButtonAttributeDrawer : IObjectEditorAttributeDrawer
    {
        private readonly List<KeyValuePair<string, MethodInfo>> _buttonMethods = new();

        public void OnEnable(Editor editor)
        {
            _buttonMethods.AddRange(EditorReflectionHelper
                .GetMethodsFromAttribute<InspectorButtonAttribute>(editor.target)
                .Select(method => new KeyValuePair<string, MethodInfo>(method.GetCustomAttribute<InspectorButtonAttribute>().ButtonName, method)));
        }

        public void OnInspectorGUI(Editor editor)
        {
            _buttonMethods
                .Where(kvp => GUILayout.Button(kvp.Key))
                .ForEach(kvp => kvp.Value.Invoke(editor.target, null));
        }
    }
}
#endif
