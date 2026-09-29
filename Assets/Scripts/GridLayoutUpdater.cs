using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GridLayoutUpdater : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup layoutGroup;
    [SerializeField] private RectTransform rect;
    [SerializeField] private int minWidth;

    /// <summary>
    /// Height / Width
    /// </summary>
    [SerializeField] private float widthToHeightRatio;
    [SerializeField] private float spacing = 10;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        UpdateGrid();
    }
    private void OnRectTransformDimensionsChange()
    {
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        float availableWidth = rect.rect.width;

        if (availableWidth <= minWidth)
            return;

        // Determine how many columns can fit.
        int columns = Mathf.FloorToInt(
            (availableWidth + spacing) /
            (minWidth + spacing)
        );

        //Limit the number of columns to the number of children
        columns = Mathf.Min(columns, transform.childCount);

        // Calculate the actual cell width so the columns fill the space.
        float cellWidth =
            (availableWidth - (columns - 1) * spacing) /
            columns;

        if (cellWidth != layoutGroup.cellSize.x)
            Debug.Log("Resized to " + cellWidth + " with " + columns + " columns and available width is " + availableWidth);

        layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layoutGroup.constraintCount = columns;

        layoutGroup.cellSize = new Vector2(cellWidth, cellWidth * widthToHeightRatio);
        layoutGroup.spacing = new Vector2(spacing, spacing);

    }
}
