#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static OnwEditor.EditorHelper;

namespace OnwEditor
{
    internal static class InspectorWindowHooker
    {
        [InitializeOnLoadMethod]
        private static void HookInspectorWindow()
        {
            Debug.Log("InspectorHook");

            var inspectorWindowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

            var trackerField = inspectorWindowType.GetField(
                "m_Tracker",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (trackerField is not null)
            {
                EditorApplication.update += () =>
                {
                    foreach (var window in Resources.FindObjectsOfTypeAll(inspectorWindowType))
                    {
                        var tracker = trackerField.GetValue(window);
                        if (tracker is not null)
                        {
                            HookDrawChain(tracker);
                        }
                    }
                };
            }
        }

        private static void HookDrawChain(object tracker)
        {
            // 여기에서 트래커의 내부 필드나 메서드를 후킹하여 커스텀 드로우 체인을 추가합니다.
            var activeEditorsField = tracker.GetType().GetProperty("activeEditors", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (activeEditorsField?.GetValue(tracker) is Editor[] activeEditors)
            {
                foreach (var editor in activeEditors)
                {
                    // 이미 후킹된 에디터인지 확인합니다.
                    if (editor.GetType().GetMethod("OnInspectorGUI") is not null)
                    {
                        // 에디터의 OnInspectorGUI를 확장합니다.
                        ExtendEditor(editor);
                    }
                }
            }
        }

        private static void ExtendEditor(Editor editor)
        {
            var originalMethod = editor.GetType().GetMethod("OnInspectorGUI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (originalMethod is not null)
            {
                var originalDelegate = (Action)Delegate.CreateDelegate(typeof(Action), editor, originalMethod);
                void customDraw()
                {
                    // 원래 OnInspectorGUI를 호출합니다.
                    originalDelegate.Invoke();

                    // 커스텀 드로우 기능을 추가합니다.
                    GUILayout.Label("Custom Draw Chain Added!", EditorStyles.boldLabel);
                }

                // OnInspectorGUI 메서드를 대체합니다.
                ReplaceMethod(editor, originalMethod, customDraw);
            }
        }

        private static void ReplaceMethod(Editor editor, MethodInfo originalMethod, Action customDraw)
        {
            var originalMethodPointer = originalMethod.MethodHandle.GetFunctionPointer();
            var customMethodPointer = customDraw.Method.MethodHandle.GetFunctionPointer();

            //unsafe
            //{
            //    if (IntPtr.Size == 4)
            //    {
            //        int* originalPtr = (int*)originalMethodPointer.ToPointer();
            //        int* customPtr = (int*)customMethodPointer.ToPointer();
            //        *originalPtr = *customPtr;
            //    }
            //    else
            //    {
            //        long* originalPtr = (long*)originalMethodPointer.ToPointer();
            //        long* customPtr = (long*)customMethodPointer.ToPointer();
            //        *originalPtr = *customPtr;
            //    }
            //}
        }
    }
}
#endif