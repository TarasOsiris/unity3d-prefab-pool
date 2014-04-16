using UnityEngine;
using System.Collections;
using System;

public static class PrefabPoolUtils
{
    public static void AddChild(GameObject parent, GameObject child)
    {
        if (child == null)
        {
            throw new NullReferenceException("Child GameObject must not be null");
        }

        Transform t = child.transform;
        t.parent = parent == null ? null : parent.transform;

        ResetTransform(t);

        if (parent != null)
        {
            child.layer = parent.layer;
        }
    }

    private static void ResetTransform(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }
}
