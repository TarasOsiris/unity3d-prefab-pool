using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMyPool : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject newParent;

    public int initialPoolSize = 15; // number of prefabs that will be instantiated when pool is created
    public int growth = 5; // number of new prefabs instantiated when the pool grows in size
    public int maxPoolSize = int.MaxValue;

    private GameObject _poolClonesContainer;
    private PrefabPool _cubesPool;

    private List<GameObject> _myInstantiatedCubes;
    void Awake()
    {
        _poolClonesContainer = GameObject.FindGameObjectWithTag("PoolHolder");
        _myInstantiatedCubes = new List<GameObject>();
    }

    private void Start()
    {
        // Create pool of cubes containing instantiated cubePrefabs
        _cubesPool = new PrefabPool(cubePrefab, _poolClonesContainer, initialPoolSize, growth, maxPoolSize);

        StartCoroutine(ObtainPoolItems());
    }

    private IEnumerator ObtainPoolItems()
    {
        // Obtain already instantiated pool items.
        const int numberOfCubesRequired = 5;
        for (int i = 0; i < numberOfCubesRequired; i++)
        {
            // obtained item becomes active the moment you retrieve it
            _myInstantiatedCubes.Add(_cubesPool.ObtainPrefabInstance());
            yield return new WaitForSeconds(0.5f);
        }

        yield return StartCoroutine(RecyclePoolItems());
    }

    private IEnumerator RecyclePoolItems()
    {
        const int numberOfCubesToRecycle = 3;
        // Recycle prefabs that are not needed at the moment
        for (int i = 0; i < numberOfCubesToRecycle; i++)
        {
            Debug.Log("recycle");
            // recycled item becomes inactive the moment you recycle it
            _cubesPool.RecyclePrefabInstance(_myInstantiatedCubes[i]);
            yield return new WaitForSeconds(0.5f);
        }
    }
}