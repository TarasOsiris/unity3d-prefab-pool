using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : IPrefabPool
{
    // ===========================================
    // Fields
    // ===========================================
    private readonly List<Transform> _availablePrefabs;
    private readonly Transform _prefab;

    // parent Transform for instantiated items 
    private readonly Transform _parent; 
    private readonly int _growth;

    // ===========================================
    // Properties
    // ===========================================
    public int UnrecycledPrefabCount { get; private set; }

    public int AvailablePrefabCount
    {
        get { return _availablePrefabs.Count; }
    }

    public int AvailablePrefabCountMaximum { get; private set; }

    // ===========================================
    // Constructors
    // ===========================================
    public PrefabPool(Transform prefab, Transform parent)
        : this(prefab, parent, 0)
    {
    }

    public PrefabPool(Transform prefab, Transform parent, int initialSize)
        : this(prefab, parent, initialSize, 1)
    {
    }

    public PrefabPool(Transform prefab, Transform parent, int initialSize, int growth)
        : this(prefab, parent, initialSize, growth, int.MaxValue)
    {
    }

    public PrefabPool(Transform prefab, Transform parent, int initialSize, int growth, int availableItemsMaximum)
    {
        if (growth <= 0)
        {
            throw new ArgumentOutOfRangeException("growth must be greater than 0!");
        }
        if (availableItemsMaximum < 0)
        {
            throw new ArgumentOutOfRangeException("availableItemsMaximum must be at least 0!");
        }

        _prefab = prefab;
        _parent = parent;
        _growth = growth;
        AvailablePrefabCountMaximum = availableItemsMaximum;
        _availablePrefabs = new List<Transform>(initialSize);

        if (initialSize > 0)
        {
            BatchAllocatePoolItems(initialSize);
        }
    }

    // ===========================================
    // Public Methods
    // ===========================================
    public Transform ObtainPrefabInstance(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Transform obtainedPrefab = ObtainPoolItem();

        obtainedPrefab.position = position;
        obtainedPrefab.rotation = rotation;
        obtainedPrefab.localScale = scale;

        return obtainedPrefab;
    }

    public void RecyclePrefabInstance(Transform prefab)
    {
        if (prefab == null)
        {
            throw new ArgumentNullException("Cannot recycle null item!");
        }

        OnHandleRecyclePrefab(prefab);

        if (_availablePrefabs.Count < AvailablePrefabCountMaximum)
        {
            _availablePrefabs.Add(prefab);
        }

        UnrecycledPrefabCount--;

        if (UnrecycledPrefabCount < 0)
        {
            Debug.Log("More items recycled than obtained");
        }
    }

    // ===========================================
    // Private Methods
    // ===========================================
    private Transform OnHandleAllocatePoolItem()
    {
        return OnAllocatePoolItem();
    }

    /*
     * Every cubePrefab that was just obtained from the pool, passes this method
     */
    private Transform OnAllocatePoolItem()
    {
        var instance = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity) as Transform;
        instance.gameObject.SetActive(false);
        instance.parent = _parent;
        return instance;
    }

    private void OnHandleObtainPrefab(Transform prefab)
    {
        prefab.gameObject.SetActive(true);
    }

    /*
     * Every item passes this method before it gets recycled
     */
    private void OnHandleRecyclePrefab(Transform prefab)
    {
        prefab.gameObject.SetActive(false);
        prefab.position = Vector3.zero;
        prefab.rotation = Quaternion.identity;
    }

    private void BatchAllocatePoolItems(int count)
    {
        List<Transform> availableItems = _availablePrefabs;

        int allocationCount = AvailablePrefabCountMaximum - availableItems.Count;
        if (count < allocationCount)
        {
            allocationCount = count;
        }

        for (int i = allocationCount - 1; i >= 0; i--)
        {
            availableItems.Add(OnAllocatePoolItem());
        }
    }

    private Transform ObtainPoolItem()
    {
        Transform prefab;

        if (_availablePrefabs.Count > 0)
        {
            prefab = RetrieveLastItemAndRemoveIt();
        }
        else
        {
            if (_growth == 1 || AvailablePrefabCountMaximum == 0)
            {
                prefab = OnHandleAllocatePoolItem();
            }
            else
            {
                BatchAllocatePoolItems(_growth);
                prefab = RetrieveLastItemAndRemoveIt();
            }

            Debug.Log(GetType().FullName + "<" + prefab.GetType().Name + "> was exhausted, with " + UnrecycledPrefabCount +
                      " items not yet recycled.  " +
                      "Allocated " + _growth + " more.");
        }

        OnHandleObtainPrefab(prefab);
        UnrecycledPrefabCount++;

        return prefab;
    }

    private Transform RetrieveLastItemAndRemoveIt()
    {
        Transform prefab;

        int lastElementIndex = _availablePrefabs.Count - 1;
        prefab = _availablePrefabs[lastElementIndex];
        _availablePrefabs.RemoveAt(lastElementIndex);

        return prefab;
    }
}