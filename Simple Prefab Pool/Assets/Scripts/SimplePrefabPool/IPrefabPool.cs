using UnityEngine;

namespace SimplePrefabPool
{
	public interface IPrefabPool
	{
	    int UnrecycledPrefabCount { get; }
	
	    int AvailablePrefabCount { get; }
	
	    int AvailablePrefabCountMaximum { get; }
	
	    GameObject ObtainPrefabInstance();
	
	    void RecyclePrefabInstance(GameObject prefab);
	}
}