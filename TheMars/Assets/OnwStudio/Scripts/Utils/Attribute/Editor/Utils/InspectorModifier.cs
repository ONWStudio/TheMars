#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Onw.Helpers;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Onw.Editor
{
    using Editor = UnityEditor.Editor;

    /// <summary>
    /// .. 리플렉션을 통해 정상적으로는 접근할 수 없는 InspectorWindow에 접근합니다
    /// 타사 라이브러리와의 충돌을 고려하여 InspectorWindow 인스펙터에 추가 기능을 담당하는 VisualElement를 Add합니다
    /// CustomEditor(typeof(MonoBehaviour)), CustomEditor(typeof(ScritableObject)) 와 같은 경우들은 MonoBehaviour, ScritableObject를 상속받는 더 구체적인 클래스의 커스텀 에디터가
    /// 구현되어있으면 target으로  MonoBehaviour, ScritableObject를 불러오지 못하는 경우가 발생합니다
    /// 커스텀 모노비하이비어, 커스텀 스크립터블 오브젝트를 구현해서 해당 클래스를 상속시키고 CustomEditor의 타겟으로 삼으면 충돌문제가 발생하지 않지만
    /// 추가 기능을 사용하려면 커스텀 클래스들을 상속받아야 한다는 약속되지 않은 규칙이 생기므로 해당 스크립트를 통해 기능들을 구현합니다
    /// 리플렉션을 통해 정상적으로는 접근할 수 없는 클래스에 접근하기 때문에 버전에 따라 동작하지 않는 경우가 발생할 수 있습니다
    /// 다른 에디터 버전에서 사용한다면 버전별 업데이트가 필요할 수 있습니다
    /// </summary>
    [InitializeOnLoad]
    internal static class InspectorWindowModifier
    {
        static InspectorWindowModifier()
        {
            EditorApplication.delayCall += ModifyInspector;
        }

        private static readonly Dictionary<string, List<IObjectEditorAttributeDrawer>> _attributeDrawers = new();

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
                // rootVisualElement를 리플렉션으로 가져옴 (2022.3.29f1) 기준 에디터 VisualElement는 editorsElement 프로퍼티
                var rootVisualElementProperty = inspectorWindowType.GetProperty("editorsElement", BindingFlags.NonPublic | BindingFlags.Instance);

                if (rootVisualElementProperty?.GetValue(inspector) is VisualElement rootVisualElement)
                {
                    // Inspector의 VisualTree를 수정하는 콜백 추가
                    rootVisualElement.RegisterCallback<GeometryChangedEvent>(evt =>
                       CallCustomDrawerMethods(rootVisualElement));
                }
            }
        }

        private static void CallCustomDrawerMethods(VisualElement root)
        {
            var editorElements = root.Query<VisualElement>(null, "unity-inspector-element").ToList();

            foreach (var editorElement in editorElements)
            {
                if (editorElement.Q<IMGUIContainer>("onw-custom-attribute-drawer") != null) continue;

                var editorField = editorElement.GetType().GetProperty("editor", BindingFlags.NonPublic | BindingFlags.Instance);

                if (editorField.GetValue(editorElement) is Editor editor &&
                    (editor.target is MonoBehaviour || editor.target is ScriptableObject))
                {
                    var target = editor.target;

                    IMGUIContainer iMGUIContainer = editorElement.Q<IMGUIContainer>();
                    iMGUIContainer.name = "onw-custom-attribute-drawer";

                    if (!_attributeDrawers.TryGetValue(editor.target.GetInstanceID().ToString(), out List<IObjectEditorAttributeDrawer> drawers))
                    {
                        drawers = new(ReflectionHelper.GetChildClassesFromType<IObjectEditorAttributeDrawer>());
                        _attributeDrawers.Add(editor.target.GetInstanceID().ToString(), drawers);
                        drawers.ForEach(drawer => drawer.OnEnable(editor));
                    }

                    foreach (IObjectEditorAttributeDrawer drawerInstance in drawers)
                    {
                        iMGUIContainer.onGUIHandler += ()
                            => drawerInstance.OnInspectorGUI(editor);
                    }

                    editorElement.Add(iMGUIContainer);
                }
            }
        }
    }
}
#endif