using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        spacing.x = Mathf.Clamp(spacing.x, 0, 1000);
        spacing.y = Mathf.Clamp(spacing.y, 0, 1000);

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;

            float sqrt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrt);
            columns = Mathf.CeilToInt(sqrt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(rectChildren.Count / (float)columns);
        }
        else if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(rectChildren.Count / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float childWidth = ((parentWidth - spacing.x * (columns - 1)) - (padding.left + padding.right)) / (float)columns;
        float childHeight = ((parentHeight - spacing.y * (rows - 1)) - (padding.top + padding.bottom)) / (float)rows;

        cellSize.x = fitX ? childWidth : cellSize.x;
        cellSize.y = fitY ? childHeight : cellSize.y;

        for (int i = 0; i < transform.childCount; i++)
        {
            int rowCount = i / columns;
            int columnCount = i % columns;

            RectTransform childRect = rectChildren[i];

            float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            float yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(childRect, 0, xPos, cellSize.x);
            SetChildAlongAxis(childRect, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
