using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("EnumeratedElementsEditors")]
#endif

[RequireComponent(typeof(GridLayoutGroup)), RequireComponent(typeof(ContentSizeFitter)), DisallowMultipleComponent]
public sealed class EnumeratedElements : MonoBehaviour
{
    [field: SerializeField] public float WidthToHeightRatio { get; set; }
    [field: SerializeField] public float ElementSizeRatio { get; set; }
    [field: SerializeField] public float ElementSizeToSpacingXRatio { get; set; }
    [field: SerializeField] public float ElementSizeToSpacingYRatio { get; set; }

    [field: SerializeField] public float PaddingLeftRatio { get; set; }
    [field: SerializeField] public float PaddingRightRatio { get; set; }
    [field: SerializeField] public float PaddingTopRatio { get; set; }
    [field: SerializeField] public float PaddingBottomRatio { get; set; }

    private ContentSizeFitter _contentSizeFitter;
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _content;

    private void Awake()
    {
        _contentSizeFitter = GetComponent<ContentSizeFitter>();
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _content = GetComponent<RectTransform>();
    }

    private IEnumerator Start()
    {
        InitGridLayoutGroup(_gridLayoutGroup);

        _contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        yield return null;

        _gridLayoutGroup.padding = GetPadding(
            _content.rect.width,
            PaddingLeftRatio,
            PaddingRightRatio,
            PaddingTopRatio,
            PaddingBottomRatio);

        _gridLayoutGroup.cellSize = GetCellSize(
            _content.rect.width,
            ElementSizeRatio,
            WidthToHeightRatio,
            _gridLayoutGroup.constraintCount);

        _gridLayoutGroup.spacing = GetSpacing(
            _gridLayoutGroup.cellSize,
            ElementSizeToSpacingXRatio,
            ElementSizeToSpacingYRatio);
    }

    internal void InitGridLayoutGroup(GridLayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    }

    internal RectOffset GetPadding(float contentWidth, float paddingLeftRatio, float paddingRightRatio, float paddingTopRatio, float paddingBottomRatio)
        => new(
            (int)(contentWidth * paddingLeftRatio),
            (int)(contentWidth * paddingRightRatio),
            (int)(contentWidth * paddingTopRatio),
            (int)(contentWidth * paddingBottomRatio));

    internal Vector2 GetCellSize(float contentWidth, float elementSizeRatio, float widthToHeightRatio, int constraintCount)
    {
        float cellWidth = contentWidth / constraintCount * elementSizeRatio;
        float cellHeight = cellWidth * widthToHeightRatio;

        return new Vector2(cellWidth, cellHeight);
    }

    internal Vector2 GetSpacing(Vector2 cellSize, float elementSizeToSpacingXRatio, float elementSizeToSpacingYRatio)
        => new(cellSize.x * elementSizeToSpacingXRatio, cellSize.y * elementSizeToSpacingYRatio);
}

