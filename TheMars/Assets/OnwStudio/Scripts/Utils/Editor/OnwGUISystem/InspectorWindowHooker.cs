#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
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
                    if (editor.GetType().GetMethod("OnInspectorGUI") is null) continue; // 이미 후킹된 에디터인지 확인합니다.

                    // 에디터의 OnInspectorGUI를 확장합니다.
                    ExtendEditor(editor);
                }
            }
        }

        private static void ExtendEditor(Editor editor)
        {
            var originalMethod = editor.GetType().GetMethod("OnInspectorGUI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (originalMethod != null)
            {
                var originalDelegate = (Action)Delegate.CreateDelegate(typeof(Action), editor, originalMethod);
                Action customDraw = () =>
                {
                    originalDelegate();
                    GUILayout.Label("Custom Draw Chain Added!", EditorStyles.boldLabel);
                };

                ReplaceVirtualMethod(editor, originalMethod, customDraw.Method);
            }
        }

        private static void ReplaceVirtualMethod(object instance, MethodInfo originalMethod, MethodInfo customMethod)
        {
            RuntimeHelpers.PrepareMethod(originalMethod.MethodHandle);
            RuntimeHelpers.PrepareMethod(customMethod.MethodHandle);

            IntPtr originalMethodPointer = originalMethod.MethodHandle.GetFunctionPointer();
            IntPtr customMethodPointer = customMethod.MethodHandle.GetFunctionPointer();

            unsafe
            {
                IntPtr* classPointer = (IntPtr*)instance.GetType().TypeHandle.Value.ToPointer();
                IntPtr* vtable = (IntPtr*)*classPointer;

                // Find the method in the vtable and replace it
                for (int i = 0; i < 100; i++) // Arbitrary limit to prevent infinite loops
                {
                    if (vtable[i] == originalMethodPointer)
                    {
                        vtable[i] = customMethodPointer;
                        break;
                    }
                }
            }
        }
    }
}
#endif