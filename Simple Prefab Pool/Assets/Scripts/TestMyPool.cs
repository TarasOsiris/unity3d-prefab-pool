using UnityEngine;

public class TestMyPool : MonoBehaviour
{
    public Transform cubePrefab;

    public Transform newParent;

    public int initialPoolSize = 15; // number of prefabs that will be instantiated when pool is created
    public int growth = 5; // number of new prefabs instantiated when the pool grows in size
    public int maxPoolSize = int.MaxValue;

    private void Start()
    {
        Transform parent = GameObject.FindGameObjectWithTag("PoolHolder").transform;

        // Create pool of cubes containing instantiated cubePrefabs
        var cubesPool = new PrefabPool(cubePrefab, parent, initialPoolSize, growth, maxPoolSize);

        // position, rotation and scale for prefab obtained from the pool
        var pos = new Vector3(2, 2, 2);
        var rotation = Quaternion.Euler(45.0f, 45.0f, 45.0f);
        var scale = new Vector3(2, 2, 2);

        // Obtain already instantiated pool items.
        const int numberOfCubesRequired = 5;
        var myCubes = new Transform[numberOfCubesRequired];

        for (int i = 0; i < myCubes.Length; i++)
        {
            // obtained item becomes active the moment you retrieve it
            myCubes[i] = cubesPool.ObtainPrefabInstance(newParent.gameObject); 
        }

        const int numberOfCubesToRecycle = 3;
        // Recycle prefabs that are not needed at the moment
        for (int i = 0; i < numberOfCubesToRecycle; i++)
        {
            // recycled item becomes inactive the moment you recycle it
            cubesPool.RecyclePrefabInstance(myCubes[i]);
        }
    }
}