using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class Spawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    public int spawnMin;
    public int spawnMax;

    public string enemyName;

    public bool bossRoom = false;
    public bool playerInBossRoom = false;

    public int enemyCounter = 0;
    public bool enemiesInRoom = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());

        if (bossRoom)
        {
            enemyCounter = 1;
        }
    }

    private void Update()
    {
        if (bossRoom)
        {
            if(enemyCounter <= 0)
            {
                StartCoroutine(LoadLastScene());
            }
        }
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

                if(enemy.name.Contains("Charger"))
                {
                    enemy.GetComponent<Charger>().spawner = this;
                }
                else if(enemy.name.Contains("Horf"))
                {
                    enemy.GetComponent<Horf>().spawner = this;
                }
                else if(enemy.name.Contains("RedBoomFly"))
                {
                    enemy.GetComponent<RedBoomFly>().spawner = this;
                }


                int index = Random.Range(0, spawnPoints.Count);
                Transform pos = spawnPoints[index];

                enemy.transform.position = pos.position;

                enemyCounter++;
                //GameController.instance.enemyKillCounter++;

                spawnPoints.Remove(pos);
            }
        }
        if (bossRoom)
        {
            GameObject enemy = PoolManager.Instance.SpawnFromPool(enemyName) as GameObject;
            enemy.transform.position = spawnPoints[0].position;

            enemy.GetComponent<DukeOfFlies>().spawner = this;
        }

    }

    IEnumerator LoadLastScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("EndScene");
    }
}
