
using System;
using UnityEngine;

public static class TransformExtension
{
    public static Transform RecursiveFindChild(this Transform parent, Func<Transform, bool> func)
    {
        if (parent == null) return null;
        if (func(parent))return parent;
        foreach (Transform child in parent)
        {
            Transform result = child.RecursiveFindChild(func);
            if (result != null) return result;
        }
        return null;
    }

    public static void DestroyAllChild(this Transform parent) {
        Transform[] childrens = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++) {
            childrens[i] = parent.GetChild(i);
        }
        foreach (Transform child in childrens) {
#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(child.gameObject);
#else
            UnityEngine.Object.Destroy(child.gameObject);
#endif
        }
    }
}