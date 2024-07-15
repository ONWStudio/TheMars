#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Editor;
using TMCard;
using Onw.Helpers;

namespace TMGUITool
{
    internal sealed partial class GUIToolDrawer : EditorWindow
    {
        public abstract class CustomInspectorEditorWindow
        {
            private Editor _editor = null;
            private readonly Dictionary<string, List<IObjectEditorAttributeDrawer>> _attrubuteDrawers = new();

            protected void OnInspectorGUI(Object target)
            {
                if (!_editor)
                {
                    _editor = Editor.CreateEditor(target);
                }

                if (_editor && !_editor.target)
                {
                    _editor.target = target;
                }

                if (!_attrubuteDrawers.TryGetValue(_editor.target.GetInstanceID().ToString(), out List<IObjectEditorAttributeDrawer> drawers))
                {
                    drawers = new(ReflectionHelper.GetChildClassesFromType<IObjectEditorAttributeDrawer>());
                    drawers.ForEach(drawer => drawer.OnEnable(_editor));
                    _attrubuteDrawers.Add(_editor.target.GetInstanceID().ToString(), drawers);
                }

                _editor.OnInspectorGUI();
                drawers
                    .ForEach(drawer => drawer.OnInspectorGUI(_editor));
            }
        }
    }
}
#endif
