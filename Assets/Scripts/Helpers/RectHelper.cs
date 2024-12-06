using System.Collections.Generic;
using UnityEngine;

public static class RectHelper 
{
    public static void ModifyRect(HashSet<Vector2> points, RectTransform rect)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var point in points)
        {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        // Ensure width and height have a minimum value
        const float minSizeThreshold = 0.01f;
        float width = Mathf.Max(maxX - minX, minSizeThreshold);
        float height = Mathf.Max(maxY - minY, minSizeThreshold);

        Vector2 position = new Vector2((minX + maxX) / 2.0f, (minY + maxY) / 2.0f);
        Vector2 size = new Vector2(width, height);

        ModifyRect(position, size.x, size.y, rect);

    }
    public static void ModifyRect(Vector2 position, float width, float height, RectTransform rect)
    {
        Vector2 normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
        Vector2 normalizedSize = new Vector2(width / Screen.width, height / Screen.height);

        // Uses normalized coordinates
        rect.anchorMin = new Vector2(normalizedPosition.x - (normalizedSize.x / 2.0f),
                                              normalizedPosition.y - (normalizedSize.y / 2.0f));
        rect.anchorMax = new Vector2(normalizedPosition.x + (normalizedSize.x / 2.0f),
                                              normalizedPosition.y + (normalizedSize.y / 2.0f));

        rect.anchoredPosition = Vector2.zero; // Position is handled by anchors
        rect.sizeDelta = Vector2.zero;        // Size is handled by anchors
    }
}
