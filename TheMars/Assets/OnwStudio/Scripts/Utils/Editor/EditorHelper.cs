#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Onw.Editor
{
    public static class EditorHelper
    {
        public static void ActionHorizontal(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            action.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void ActionHorizontal(Action action, GUIStyle guiStyle, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(guiStyle, options);
            action.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void ActionEditorHorizontal(Action action, GUIStyle guiStyle, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(guiStyle, options);
            action.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void ActionEditorHorizontal(Action action, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            action.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void ActionEditorVertical(Action action, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void ActionEditorVertical(Action action, GUIStyle guiStyle, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(guiStyle, options);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void ActionVertical(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            action.Invoke();
            GUILayout.EndVertical();
        }

        public static void ActionVertical(Action action, GUIStyle guiStyle, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(guiStyle, options);
            action.Invoke();
            GUILayout.EndVertical();
        }

        public static void ActionEditorVerticalBox(GUIStyle guiStyle, ref Vector2 scrollPosition, Action action, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(guiStyle, options);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            action.Invoke();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public static void ActionVerticalBox(GUIStyle guiStyle, ref Vector2 scrollPosition, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(guiStyle, options);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            action.Invoke();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        public static object GetPropertyValue(SerializedProperty property) => property?.propertyType switch
        {
            SerializedPropertyType.Generic or SerializedPropertyType.ManagedReference => property.managedReferenceValue,
            SerializedPropertyType.Integer or SerializedPropertyType.LayerMask or SerializedPropertyType.Character => property.intValue,
            SerializedPropertyType.Boolean => property.boolValue,
            SerializedPropertyType.Float => property.floatValue,
            SerializedPropertyType.String => property.stringValue,
            SerializedPropertyType.Color => property.colorValue,
            SerializedPropertyType.ObjectReference => property.objectReferenceValue,
            SerializedPropertyType.Enum => property.enumNames[property.enumValueIndex],
            SerializedPropertyType.Vector2 => property.vector2Value,
            SerializedPropertyType.Vector3 => property.vector3Value,
            SerializedPropertyType.Vector4 => property.vector4Value,
            SerializedPropertyType.Rect => property.rectValue,
            SerializedPropertyType.ArraySize => property.arraySize,
            SerializedPropertyType.AnimationCurve => property.animationCurveValue,
            SerializedPropertyType.Bounds => property.boundsValue,
            SerializedPropertyType.Gradient => property.gradientValue,
            SerializedPropertyType.Quaternion => property.quaternionValue,
            SerializedPropertyType.ExposedReference => property.exposedReferenceValue,
            SerializedPropertyType.FixedBufferSize => property.fixedBufferSize,
            SerializedPropertyType.Vector2Int => property.vector2IntValue,
            SerializedPropertyType.Vector3Int => property.vector3IntValue,
            SerializedPropertyType.RectInt => property.rectIntValue,
            SerializedPropertyType.BoundsInt => property.boundsIntValue,
            SerializedPropertyType.Hash128 => property.hash128Value,
            _ => null,
        };

        public static object GetPropertyValueFromObject(object obj) => obj switch
        {
            int intValue => intValue,
            bool boolValue => boolValue,
            float floatValue => floatValue,
            string stringValue => stringValue,
            Color colorValue => colorValue,
            UnityEngine.Object objectReferenceValue => objectReferenceValue,
            Enum enumValue => enumValue.ToString(),
            Vector2 vector2Value => vector2Value,
            Vector3 vector3Value => vector3Value,
            Vector4 vector4Value => vector4Value,
            Rect rectValue => rectValue,
            AnimationCurve animationCurveValue => animationCurveValue,
            Bounds boundsValue => boundsValue,
            Quaternion quaternionValue => quaternionValue,
            Vector2Int vector2IntValue => vector2IntValue,
            Vector3Int vector3IntValue => vector3IntValue,
            RectInt rectIntValue => rectIntValue,
            BoundsInt boundsIntValue => boundsIntValue,
            Hash128 hash128Value => hash128Value,
            IEnumerable enumerable => enumerable.Cast<object>().Count(),
            _ => null,
        };

        public static void DestroyObject(GameObject go)
        {
            if (!go) return;

            UnityEngine.Object.DestroyImmediate(go);
        }

        public static void DestroyObjectByComponent<T>(T component) where T : Component
        {
            if (!component) return;

            DestroyObject(component.gameObject);
        }

        /// <summary>
        /// .. component를 참조로 가져와서 Destroy후 Null초기화를 합니다
        /// </summary>
        /// <param name="component"></param>
        public static void DestroyObjectByComponent<T>(ref T component) where T : Component
        {
            if (!component) return;

            DestroyObject(component.gameObject);
            component = null;
        }

        public static void DestroyTexture<T>(T texture) where T : Texture
        {
            if (!texture) return;

            UnityEngine.Object.DestroyImmediate(texture);
        }

        /// <summary>
        /// .. Texture를 참조로 가져와서 Destroy후 Null초기화를 합니다
        /// </summary>
        /// <param name="texture"></param>
        public static void DestroyTexture<T>(ref T texture) where T : Texture
        {
            if (!texture) return;

            UnityEngine.Object.DestroyImmediate(texture);
            texture = null;
        }

        public static void ReleaseRenderTexture(RenderTexture renderTexture)
        {
            if (!renderTexture) return;

            renderTexture.Release();
        }

        /// <summary>
        /// .. RenderTexture를 참조로 가져와서 Release후 Null초기화를 합니다
        /// </summary>
        /// <param name="renderTexture"></param>
        public static void ReleaseRenderTexture(ref RenderTexture renderTexture)
        {
            if (!renderTexture) return;

            renderTexture.Release();
            renderTexture = null;
        }

        /// <summary>
        /// .. 어떤 데이터 리스트가 있을 때 해당 데이터들의 요소를 토글 기능을 가진 스크롤뷰에 수직으로 열거시키는 기능 입니다 열거된 데이터에서 토글 선택된 데이터 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="T"> .. 데이터 요소의 타입 </typeparam>
        /// <param name="targets"> .. 열거시킬 데이터 리스트</param>
        /// <param name="text">.. 요소를 가져와서 해당 요소로 text를 쓸 수 있게하는 콜백 함수 입니다 </param>
        /// <param name="chooseTarget"> .. 열거된 데이터리스트 중에서 선택되어 있는 변수를 넣어주어야 합니다 </param>
        /// <param name="selectGUIStyle"> .. 어떤 요소가 선택됐을때 해당 요소를 GUI에 어떻게 표현할 것인지를 결정하는 변수 </param>
        /// <param name="options"> .. GUILayout메서드들에 인자값으로 들어가는 options와 같습니다. </param>
        /// <returns> .. 열거된 데이터 리스트 중에서 토글 선택된 요소 </returns>
        public static T SelectEnumeratedToggles<T>(IEnumerable<T> targets, Func<T, string> text, T chooseTarget, GUIStyle selectGUIStyle, GUIStyle defaultGUIStyle, params GUILayoutOption[] options)
        {
            T selectingTarget = chooseTarget;

            foreach (T target in targets)
            {
                bool isTargeting = chooseTarget?.Equals(target) ?? false;
                bool isSelected = GUILayout.Toggle(isTargeting, text.Invoke(target), isTargeting ? selectGUIStyle : defaultGUIStyle, options);

                if (isSelected && !isTargeting)
                {
                    selectingTarget = target;
                }
                if (!isSelected && isTargeting)
                {
                    selectingTarget = default;
                }
            }

            return selectingTarget;
        }

        /// <summary>
        /// .. guiStyle의 텍스쳐를 커스텀하여 사용하고 싶을때 사용할 수 있는 함수 입니다. 커스텀할 guiStyle을 인자값으로 넣어주어야 합니다.
        /// </summary>
        /// <param name="color"> .. 변경할 컬러 </param>
        /// <param name="guiStyle"> .. 커스텀 할 GUIStyle </param>
        /// <returns></returns>
        public static Texture2D GetTexture2D(Color32 color, GUIStyle guiStyle)
        {
            Color32[] pix = new Color32[guiStyle.border.horizontal * guiStyle.border.vertical];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }

            Texture2D texture = new(guiStyle.border.horizontal, guiStyle.border.vertical);
            texture.SetPixels32(pix);
            texture.Apply();

            return texture;
        }

        public static void DisableEditorWindow<T>(ref T editorWindow) where T : EditorWindow
        {
            if (editorWindow)
            {
                editorWindow.Close();
            }
            editorWindow = null;
        }

        public static void OnErrorMessage(ref string errorMessage, GUIStyle labelStyle)
        {
            if (errorMessage is null) return;

            GUILayout.Label(errorMessage, labelStyle);
            Debug.LogWarning(errorMessage);

            if (GUILayout.Button("OK", GUILayout.Width(30)))
            {
                errorMessage = null;
            }
        }
        /// <summary>
        /// .. 자동구현 프로퍼티의 백킹 필드의 직렬화 된 이름을 가져옵니다.
        /// </summary>
        /// <param name="propertyName">
        /// 프로퍼티 이름
        /// </param>
        /// <returns></returns>
        public static string GetBackingFieldName(string propertyName) => $"<{propertyName}>k__BackingField";

        public static SerializedProperty GetProperty(SerializedObject serializedObject, string propertyName)
        {
            return serializedObject.FindProperty(propertyName) ??
                serializedObject.FindProperty(GetBackingFieldName(propertyName));
        }
    }
}
#endif