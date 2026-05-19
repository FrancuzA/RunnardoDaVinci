using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void DestroyAllChildren(this Transform t, List<Transform> excludes = null)
    {
        if (excludes == null)
        {
            foreach (Transform child in t) Object.Destroy(child.gameObject);
            return;
        }

        foreach (Transform child in t)
        {
            if (excludes.Contains(child))
                continue;
            Object.Destroy(child.gameObject);
        }
    }

    public static bool ToBool(this int value) => value == 1;

    public static void SetHeight(this RectTransform rt, float height)
        => rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

    public static void SetWidth(this RectTransform rt, float width)
        => rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
}