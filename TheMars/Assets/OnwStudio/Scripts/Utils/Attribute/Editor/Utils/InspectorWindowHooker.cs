#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Helpers;
using UnityEngine;
using UnityEditor;
using static Onw.Editor.EditorHelper;
using UnityEngine.UIElements;

namespace Onw.Editor
{
    using Editor = UnityEditor.Editor;

    [InitializeOnLoad]
    internal static class InspectorWindowModifier
    {
        static InspectorWindowModifier()
        {
            _attributeDrawers.AddRange(ReflectionHelper.GetChildClassesFromType<IObjectEditorAttributeDrawer>());
            EditorApplication.delayCall += ModifyInspector;
        }

        private static readonly List<IObjectEditorAttributeDrawer> _attributeDrawers = new();

        private static void ModifyInspector()
        {
            // InspectorWindow 타입을 얻어옴
            var inspectorWindowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            if (inspectorWindowType == null) return;

            // 모든 InspectorWindow 인스턴스를 얻어옴
            var inspectors = Resources
                .FindObjectsOfTypeAll(inspectorWindowType)
                .OfType<EditorWindow>();

            foreach (var inspector in inspectors)
            {
                // rootVisualElement를 리플렉션으로 가져옴
                var rootVisualElementProperty = inspectorWindowType.GetProperty("editorsElement", BindingFlags.NonPublic | BindingFlags.Instance);

                if (rootVisualElementProperty.GetValue(inspector) is VisualElement rootVisualElement)
                {
                    // Inspector의 VisualTree를 수정하는 콜백 추가
                    rootVisualElement.RegisterCallback<GeometryChangedEvent>(evt =>
                        CallCustomDrawerMethods(rootVisualElement, inspector));
                }
            }
        }

        private static void CallCustomDrawerMethods(VisualElement root, object inspector)
        {
            var editorElements = root.Query<VisualElement>(null, "unity-inspector-element").ToList();
            // 이미 호출된 경우 중복 방지

            foreach (var editorElement in editorElements)
            {
                if (editorElement.Q("onw-custom-attribute-drawer") != null) continue;

                var editorField = editorElement.GetType().GetProperty("editor", BindingFlags.NonPublic | BindingFlags.Instance);

                Debug.Log("Draw Call! Custom");

                if (editorField.GetValue(editorElement) is Editor editor &&
                    (editor.target is MonoBehaviour || editor.target is ScriptableObject))
                {
                    var target = editor.target;

                    IMGUIContainer iMGUIContainer = editorElement.Q<IMGUIContainer>();
                    iMGUIContainer.name = "onw-custom-attribute-drawer";
                    foreach (IObjectEditorAttributeDrawer drawerInstance in _attributeDrawers)
                    {
                        drawerInstance.OnEnable(editor);

                        iMGUIContainer.onGUIHandler += () =>
                            drawerInstance.OnInspectorGUI(editor);
                    }

                    editorElement.Add(iMGUIContainer);

                }
            }
        }
    }
}
#endif