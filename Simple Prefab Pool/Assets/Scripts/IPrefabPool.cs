using UnityEngine;

public interface IPrefabPool
{
    int UnrecycledPrefabCount { get; }

    int AvailablePrefabCount { get; }

    int AvailablePrefabCountMaximum { get; }

    Transform ObtainPrefabInstance(Vector3 position, Quaternion rotation, Vector3 scale);

    void RecyclePrefabInstance(Transform prefab);
}