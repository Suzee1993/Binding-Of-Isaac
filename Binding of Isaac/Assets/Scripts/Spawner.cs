using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    public int spawnMin;
    public int spawnMax;

    public string enemyName;

    public bool bossRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        if (!bossRoom)
        {
            int amountToSpawn = Random.Range(spawnMin, spawnMax);

            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject enemy = PoolManager.Instance.SpawnFromPool(enemyName) as GameObject;

                int index = Random.Range(0, spawnPoints.Count);
                Transform pos = spawnPoints[index];

                enemy.transform.position = pos.position;

                spawnPoints.Remove(pos);
            }
        }

    }
}
