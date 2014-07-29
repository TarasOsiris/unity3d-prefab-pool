using SimplePrefabPool;
using UnityEngine;
using System.Collections;

public class MyCubePool : AbstractPrefabPool<MyCube>
{
    public MyCubePool(MyCube objectToCopy, GameObject parent) : base(objectToCopy, parent)
    {
    }

    public MyCubePool(MyCube objectToCopy, GameObject parent, int initialSize) : base(objectToCopy, parent, initialSize)
    {
    }

    public MyCubePool(MyCube objectToCopy, GameObject parent, int initialSize, int growth) : base(objectToCopy, parent, initialSize, growth)
    {
    }

    public MyCubePool(MyCube objectToCopy, GameObject parent, int initialSize, int growth, int availableItemsMaximum) : base(objectToCopy, parent, initialSize, growth, availableItemsMaximum)
    {
    }

    protected override void OnHandleAllocatePrefab(MyCube prefabInstance)
    {
        prefabInstance.gameObject.SetActive(true);
        prefabInstance.renderer.material.color = Color.yellow;
        prefabInstance.transform.position = new Vector3(-3, -3, -3);
    }

    protected override void OnHandleObtainPrefab(MyCube prefabInstance)
    {
        prefabInstance.gameObject.SetActive(true);
        prefabInstance.renderer.material.color = Color.green;
    }

    protected override void OnHandleRecyclePrefab(MyCube prefabInstance)
    {
        prefabInstance.gameObject.SetActive(true);
        prefabInstance.renderer.material.color = Color.red;
    }
}