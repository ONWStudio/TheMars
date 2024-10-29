// #if UNITY_EDITOR
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using Unity.EditorCoroutines;
// using Onw.Editor;
// using Onw.Helper;
// using Onw.Editor.Extensions;
// using Unity.EditorCoroutines.Editor;
//
// namespace TMGuiTool
// {
//     internal sealed partial class GUIToolDrawer : EditorWindow
//     {
//         public abstract class CustomInspectorEditorWindow
//         {
//             private Editor _editor = null;
//             private readonly Dictionary<string, List<IObjectEditorAttributeDrawer>> _attributeDrawers = new();
//
//             protected void OnInspectorGUI(Object target)
//             {
//                 if (!_editor)
//                 {
//                     createEditor(target);
//                 }
//
//                 if (!_editor.target || _editor.target != target || !_editor.serializedObject.targetObject)
//                 {
//                     destroyEditor();
//                     createEditor(target);
//                 }
//
//                 if (!_attributeDrawers.TryGetValue(_editor.target.GetInstanceID().ToString(), out List<IObjectEditorAttributeDrawer> drawers))
//                 {
//                     drawers = new(ReflectionHelper.CreateChildClassesFromType<IObjectEditorAttributeDrawer>());
//                     drawers.ForEach(drawer => drawer.OnEnable(_editor));
//                     _attributeDrawers.Add(_editor.target.GetInstanceID().ToString(), drawers);
//                 }
//
//                 _editor.OnInspectorGUI();
//                 drawers
//                     .ForEach(drawer => drawer.OnInspectorGUI(_editor));
//             }
//
//             private void createEditor(Object target)
//             {
//                 _editor = Editor.CreateEditor(target);
//                 Object prevObject = Selection.activeObject;
//                 EditorApplication.delayCall += () => Selection.activeObject = prevObject;
//                 Selection.activeObject = target;
//             }
//
//             private IEnumerator iESetSelectionActiveObjectPrevTarget(Object prevTarget)
//             {
//                 yield return null;
//                 Selection.activeObject = prevTarget;
//             }
//
//             public void OnDisable()
//             {
//                 destroyEditor();
//             }
//
//             private void destroyEditor()
//             {
//                 if (!_editor) return;
//
//                 DestroyImmediate(_editor);
//                 _editor = null;
//             }
//         }
//     }
// }
// #endif
