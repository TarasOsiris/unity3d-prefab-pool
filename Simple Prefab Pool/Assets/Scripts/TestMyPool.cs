using UnityEngine;

public class TestMyPool : MonoBehaviour
{
    public Transform cubePrefab;

    private void Start()
    {
        int initialPoolSize = 15;
        int growth = 5; // number of new prefabs instantiated when the pool grows in size
        int maxPoolSize = int.MaxValue;

        PrefabPool cubesPool = new PrefabPool(cubePrefab, initialPoolSize, growth, maxPoolSize);

        Vector3 pos = new Vector3(2, 2, 2);
        Quaternion rotation = Quaternion.Euler(45.0f, 45.0f, 45.0f);
        Vector3 scale = new Vector3(2, 2, 2);

        // Obtain already instantiated pool items.
        const int numberOfCubesRequired = 5;
        Transform[] myCubes = new Transform[numberOfCubesRequired];

        for (int i = 0; i < myCubes.Length; i++)
        {
            myCubes[i] = cubesPool.ObtainPrefab(pos*i, rotation, scale);
        }

        const int numberOfCubesToRecycle = 3;
        // Recycle prefabs that are not needed at the moment
        for (int i = 0; i < numberOfCubesToRecycle; i++)
        {
            cubesPool.RecyclePrefab(myCubes[i]);
        }
    }
}