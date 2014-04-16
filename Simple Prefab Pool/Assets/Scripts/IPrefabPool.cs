using UnityEngine;

public interface IPrefabPool
{
    int UnrecycledPrefabCount { get; }

    int AvailablePrefabCount { get; }

    int AvailablePrefabCountMaximum { get; }

    Transform ObtainPrefabInstance(GameObject newParent);

    void RecyclePrefabInstance(Transform prefab);
}