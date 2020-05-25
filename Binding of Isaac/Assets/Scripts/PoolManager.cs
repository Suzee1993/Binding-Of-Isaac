using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static PoolManager Instance;

    public readonly Dictionary<string, GameObject> ResourceCache = new Dictionary<string, GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, new Vector3(0, 0, 0), Quaternion.identity);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag: {tag} does not exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
