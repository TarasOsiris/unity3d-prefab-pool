using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimplePrefabPool
{
	public class PrefabPool : IPrefabPool
	{
	    private readonly List<GameObject> _availableInstances;
	    private readonly GameObject _prefab;
	
	    // parent GameObject for instantiated items
	    private readonly GameObject _parent;
	    private readonly int _growth;
	
	    public int UnrecycledPrefabCount { get; private set; }
	
	    public int AvailablePrefabCount
	    {
	        get { return _availableInstances.Count; }
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
	        _availableInstances = new List<GameObject>(initialSize);
	
	        if (initialSize > 0)
	        {
	            BatchAllocatePoolItems(initialSize);
	        }
	    }
        #endregion

	    public GameObject ObtainPrefabInstance()
	    {
            GameObject prefabInstance;

            if (_availableInstances.Count > 0)
            {
                prefabInstance = RetrieveLastItemAndRemoveIt();
            }
            else
            {
                if (_growth == 1 || AvailablePrefabCountMaximum == 0)
                {
                    prefabInstance = AllocatePoolItem();
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

        /*
         * Every item passes this method before it is obtained from the pool
         */
        protected virtual void OnHandleObtainPrefab(GameObject prefabInstance)
        {
            prefabInstance.gameObject.SetActive(true);
        }
	
	    public void RecyclePrefabInstance(GameObject prefab)
	    {
	        if (prefab == null) { throw new ArgumentNullException("Cannot recycle null item!"); }
	
	        OnHandleRecyclePrefab(prefab);

	        if (_availableInstances.Count < AvailablePrefabCountMaximum) { _availableInstances.Add(prefab); }
	        UnrecycledPrefabCount--;
	
	        if (UnrecycledPrefabCount < 0) { Debug.Log("More items recycled than obtained"); }
	    }

	    /*
	     * Every item passes this method before it gets recycled
	     */
	    protected virtual void OnHandleRecyclePrefab(GameObject prefabInstance)
	    {
	        PrefabPoolUtils.AddChild(_parent.gameObject, prefabInstance.gameObject);
	        prefabInstance.gameObject.SetActive(false);
	    }

	    private GameObject AllocatePoolItem()
	    {
	        var instance = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity) as GameObject;
	        instance.gameObject.SetActive(false);
	        instance.transform.parent = _parent.transform;
	        return instance;
	    }

	    #region herlper_methods
	    private void BatchAllocatePoolItems(int count)
	    {
	        List<GameObject> availableItems = _availableInstances;
	
	        int allocationCount = AvailablePrefabCountMaximum - availableItems.Count;
	        if (count < allocationCount)
	        {
	            allocationCount = count;
	        }
	
	        for (int i = allocationCount - 1; i >= 0; i--)
	        {
	            availableItems.Add(AllocatePoolItem());
	        }
	    }

	    private GameObject RetrieveLastItemAndRemoveIt()
	    {
	        int lastElementIndex = _availableInstances.Count - 1;
	        var prefab = _availableInstances[lastElementIndex];
	        _availableInstances.RemoveAt(lastElementIndex);
	
	        return prefab;
        }
        #endregion
    }
}