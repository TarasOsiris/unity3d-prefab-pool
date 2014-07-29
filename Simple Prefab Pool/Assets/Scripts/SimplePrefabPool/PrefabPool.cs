using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePrefabPool
{
	public class PrefabPool : IPrefabPool
	{
	    private readonly List<GameObject> _availablePrefabs;
	    private readonly GameObject _prefab;
	
	    // parent GameObject for instantiated items
	    private readonly GameObject _parent;
	    private readonly int _growth;
	
	    public int UnrecycledPrefabCount { get; private set; }
	
	    public int AvailablePrefabCount
	    {
	        get { return _availablePrefabs.Count; }
	    }
	
	    public int AvailablePrefabCountMaximum { get; private set; }

        #region constructors
        public PrefabPool(GameObject prefab, GameObject parent)
	        : this(prefab, parent, 0)
	    {
	    }
	
	    public PrefabPool(GameObject prefab, GameObject parent, int initialSize)
	        : this(prefab, parent, initialSize, 1)
	    {
	    }
	
	    public PrefabPool(GameObject prefab, GameObject parent, int initialSize, int growth)
	        : this(prefab, parent, initialSize, growth, int.MaxValue)
	    {
	    }
	
	    public PrefabPool(GameObject prefab, GameObject parent, int initialSize, int growth, int availableItemsMaximum)
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
	        _availablePrefabs = new List<GameObject>(initialSize);
	
	        if (initialSize > 0)
	        {
	            BatchAllocatePoolItems(initialSize);
	        }
	    }
        #endregion

        // ===========================================
	    // Public Methods
	    // ===========================================
	
	    #region API
	
	    public GameObject ObtainPrefabInstance()
	    {
	        return ObtainPoolItem();
	    }
	
	    public void RecyclePrefabInstance(GameObject prefab)
	    {
	        if (prefab == null) { throw new ArgumentNullException("Cannot recycle null item!"); }
	
	        OnHandleRecyclePrefab(prefab);
	        if (_availablePrefabs.Count < AvailablePrefabCountMaximum) { _availablePrefabs.Add(prefab); }
	        UnrecycledPrefabCount--;
	
	        if (UnrecycledPrefabCount < 0) { Debug.Log("More items recycled than obtained"); }
	    }
	
	    #endregion
	
	    /*
	     * Every cubePrefab that was just obtained from the pool, passes this method
	     */
	    private GameObject OnAllocatePoolItem()
	    {
	        var instance = GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity) as GameObject;
	        instance.gameObject.SetActive(false);
	        instance.transform.parent = _parent.transform;
	        return instance;
	    }
	
	    private void OnHandleObtainPrefab(GameObject prefabInstance)
	    {
	        prefabInstance.gameObject.SetActive(true);
	    }
	
	    /*
	     * Every item passes this method before it gets recycled
	     */
	    private void OnHandleRecyclePrefab(GameObject prefabInstance)
	    {
	        PrefabPoolUtils.AddChild(_parent.gameObject, prefabInstance.gameObject);
	        prefabInstance.gameObject.SetActive(false);
	    }
	
	    private void BatchAllocatePoolItems(int count)
	    {
	        List<GameObject> availableItems = _availablePrefabs;
	
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
	
	    private GameObject ObtainPoolItem()
	    {
	        GameObject prefabInstance;
	
	        if (_availablePrefabs.Count > 0)
	        {
	            prefabInstance = RetrieveLastItemAndRemoveIt();
	        }
	        else
	        {
	            if (_growth == 1 || AvailablePrefabCountMaximum == 0)
	            {
	                prefabInstance = OnAllocatePoolItem();
	            }
	            else
	            {
	                BatchAllocatePoolItems(_growth);
	                prefabInstance = RetrieveLastItemAndRemoveIt();
	            }
	
	            Debug.Log(GetType().FullName + "<" + prefabInstance.GetType().Name + "> was exhausted, with " + UnrecycledPrefabCount +
	            " items not yet recycled.  " +
	            "Allocated " + _growth + " more.");
	        }
	
	        OnHandleObtainPrefab(prefabInstance);
	        UnrecycledPrefabCount++;
	
	        return prefabInstance;
	    }
	
	    private GameObject RetrieveLastItemAndRemoveIt()
	    {
	        int lastElementIndex = _availablePrefabs.Count - 1;
	        var prefab = _availablePrefabs[lastElementIndex];
	        _availablePrefabs.RemoveAt(lastElementIndex);
	
	        return prefab;
	    }
	}
}