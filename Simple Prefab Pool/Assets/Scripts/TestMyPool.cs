using UnityEngine;

public class TestMyPool : MonoBehaviour
{
    public Transform cubePrefab;

    private void Start()
    {
        int initialPoolSize = 15; // number of prefabs that will be instantiated when pool is created
        int growth = 5; // number of new prefabs instantiated when the pool grows in size
        int maxPoolSize = int.MaxValue;

        Transform parent = GameObject.FindGameObjectWithTag("PoolHolder").transform;

        // Create pool of cubes containing instantiated cubePrefabs
        PrefabPool cubesPool = new PrefabPool(cubePrefab, parent, initialPoolSize, growth, maxPoolSize);

        // position, rotation and scale for prefab obtained from the pool
        Vector3 pos = new Vector3(2, 2, 2);
        Quaternion rotation = Quaternion.Euler(45.0f, 45.0f, 45.0f);
        Vector3 scale = new Vector3(2, 2, 2);

        // Obtain already instantiated pool items.
        const int numberOfCubesRequired = 5;
        Transform[] myCubes = new Transform[numberOfCubesRequired];

        for (int i = 0; i < myCubes.Length; i++)
        {
            // obtained item becomes active the moment you retrieve it
            myCubes[i] = cubesPool.ObtainPrefab(pos * i, rotation, scale); 
        }

        const int numberOfCubesToRecycle = 3;
        // Recycle prefabs that are not needed at the moment
        for (int i = 0; i < numberOfCubesToRecycle; i++)
        {
            // recycled item becomes inactive the moment you recycle it
            cubesPool.RecyclePrefab(myCubes[i]);
        }
    }
}