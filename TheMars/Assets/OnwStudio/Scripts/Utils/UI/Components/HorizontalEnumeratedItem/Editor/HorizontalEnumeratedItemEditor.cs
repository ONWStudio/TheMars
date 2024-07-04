#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.UI.Components;
using static Onw.Editor.EditorHelper;
using static Onw.UI.Components.HorizontalEnumeratedItem;

[CustomEditor(typeof(HorizontalEnumeratedItem))]
internal sealed class HorizontalEnumeratedItemEditor : Editor
{
    private HorizontalEnumeratedItem _horizontalEnumeratedItem;

    private SerializedProperty _viewport;
    private SerializedProperty _content;

    private SerializedProperty _spacingRatio;
    private SerializedProperty _itemHeightRatioFromContentHeight;
    private SerializedProperty _itemWidthRatioFromHeight;
    private SerializedProperty _onPointerUpCorrection;
    private SerializedProperty _elasticity;

    private SerializedProperty _selectedIndex;

    private SerializedProperty _onChangedValue;

    [MenuItem("GameObject/Create/UI/Horizontal Enumerated Item")]
    public static void CreateHorizontalEnumeratedItem(MenuCommand menuCommand)
    {
        GameObject horizontalEnumeratedPrefab = Resources.Load<GameObject>("HorizontalEnumeratedItem");
        GameObject newUIInstance = Instantiate(horizontalEnumeratedPrefab, (menuCommand.context as GameObject).transform);
        newUIInstance.name = "HorizontalEnumeratedItem";

        Undo.RegisterCreatedObjectUndo(newUIInstance, "Create Horizontal Enumerated Item");
        Selection.activeObject = newUIInstance;
    }

    private void OnEnable()
    {
        _horizontalEnumeratedItem = target as HorizontalEnumeratedItem;
        _viewport = serializedObject.FindProperty(GetBackingFieldName("Viewport"));
        _content = serializedObject.FindProperty(GetBackingFieldName("Content"));
        _onChangedValue = serializedObject.FindProperty(GetBackingFieldName("OnChangedValue"));
        _spacingRatio = serializedObject.FindProperty("_spacingRatio");
        _itemHeightRatioFromContentHeight = serializedObject.FindProperty("_itemHeightRatioFromContentHeight");
        _itemWidthRatioFromHeight = serializedObject.FindProperty("_itemWidthRatioFromHeight");
        _onPointerUpCorrection = serializedObject.FindProperty("_onPointerUpCorrection");
        _elasticity = serializedObject.FindProperty("_elasticity");
        _selectedIndex = serializedObject.FindProperty("_selectedIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ActionEditorVertical(() =>
        {
            EditorGUILayout.PropertyField(_viewport, new GUIContent("Viewport", "열거된 아이템을 보여줄 UI 입니다 RectMask2D와 EventTrigger를 사용해주세요"), true);
            EditorGUILayout.PropertyField(_content, new GUIContent("Content", "실제 열거되어있는 아이템을 보관할 컨텐츠 입니다. 스크롤에 관한 기능은 컨텐츠를 사용합니다."), true);
        }, GUI.skin.box);

        if (_viewport.objectReferenceValue && _content.objectReferenceValue)
        {
            RectTransform content = _content.objectReferenceValue as RectTransform;

            EditorGUILayout.LabelField("Selected Index");
            ActionEditorVertical(() => EditorGUILayout.IntSlider(_selectedIndex, 0, content.childCount - 1, new GUIContent("Selected Index", ".. 현재 선택된 인덱스를 반환 값 변경시 OnChangedValue 이벤트 발생\n " +
                "이벤트를 발생시키지 않으려면 SetSelectedIndexWithOutNotify 메서드를 사용해주세요")), GUI.skin.box);

            EditorGUILayout.LabelField("Content Option");
            ActionEditorVertical(() =>
            {
                EditorGUILayout.Slider(
                    _onPointerUpCorrection,
                    ON_POINTER_UP_CORRECTION_LIMIT.Min,
                    ON_POINTER_UP_CORRECTION_LIMIT.Max,
                    new GUIContent("OnPointerUpCorrection", "드래그 하다 놓을 시 중앙에 가까운 위치에 있는 아이템으로의 보정 속도"));

                EditorGUILayout.Slider(
                    _elasticity,
                    ELASTICITY_LIMIT.Min,
                    ELASTICITY_LIMIT.Max,
                    new GUIContent("Elasticity", "뷰포트 내부의 콘텐츠가 뷰포트 범위에서 벗어났을때 원래대로 돌아가려는 힘"));
            }, GUI.skin.box);

            EditorGUILayout.LabelField("Item Option");
            ActionEditorVertical(() =>
            {
                EditorGUILayout.Slider(
                    _spacingRatio,
                    SPACING_RATIO_LIMIT.Min,
                    SPACING_RATIO_LIMIT.Max,
                    new GUIContent("SpacingRatio", "각 아이템은 어느정도의 간격으로 배치될건지의 비율"));

                EditorGUILayout.Slider(
                    _itemHeightRatioFromContentHeight,
                    ITEM_HEIGHT_RATIO_FROM_CONTENT_HEIGHT_LIMIT.Min,
                    ITEM_HEIGHT_RATIO_FROM_CONTENT_HEIGHT_LIMIT.Max,
                    new GUIContent("ItemHeightRatioFromContentHeight", "아이템의 높이를 Content 높이의 기준의 비율로 정하는 값"));

                EditorGUILayout.Slider(
                    _itemWidthRatioFromHeight,
                    ITEM_WIDTH_RATIO_FROM_HEIGHT_LIMIT.Min,
                    ITEM_WIDTH_RATIO_FROM_HEIGHT_LIMIT.Max,
                    new GUIContent("ItemWidthRatioFromHeight", "아이템의 넓이를 높이 기준의 비율로 정하는 값"));

                if (!Application.isPlaying)
                {
                    _horizontalEnumeratedItem.Init(_viewport.objectReferenceValue as RectTransform, content);

                    if (content.childCount > 0)
                    {
                        content.localPosition = _horizontalEnumeratedItem.GetContentLocalPositionFromSelectedIndex(
                            _selectedIndex.intValue,
                            content.childCount,
                            content.sizeDelta.x);
                    }
                }
            }, GUI.skin.box);
        }

        EditorGUILayout.LabelField("Event");
        EditorGUILayout.PropertyField(_onChangedValue, new GUIContent("OnChangedValue", "선택된 아이템이 변경될시 해당 아이템의 인덱스를 이벤트로 넘겨줍니다"), true);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
