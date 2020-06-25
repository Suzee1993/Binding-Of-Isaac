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

    public Room room;

    private bool hasSpawned = false;

    void Start()
    {
        if (bossRoom)
        {
            enemyCounter = 1;
        }
    }

    private void Update()
    {
        if (room.playerInRoom && !hasSpawned)
        {
            hasSpawned = true;

            StartCoroutine(Wait());
        }

        if (bossRoom)
        {
            StartCoroutine(CheckForLastEnemy());


        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.3f);
        if (!bossRoom)
        {
            int amountToSpawn = Random.Range(spawnMin, spawnMax);

            for (int i = 0; i < amountToSpawn; i++)
            {

                GameObject enemy = PoolManager.Instance.SpawnFromPool(enemyName) as GameObject;

                if (enemy.name.Contains("Charger"))
                {
                    Charger charger = enemy.GetComponent<Charger>();
                    charger.spawner = this;
                    room.enemiesInRoomList.Add(this.gameObject);
                }
                else if (enemy.name.Contains("Horf"))
                {
                    Horf horf = enemy.GetComponent<Horf>();
                    horf.spawner = this;
                    room.enemiesInRoomList.Add(this.gameObject);
                }
                else if (enemy.name.Contains("RedBoomFly"))
                {
                    RedBoomFly redBoomFly = enemy.GetComponent<RedBoomFly>();
                    redBoomFly.spawner = this;
                    room.enemiesInRoomList.Add(this.gameObject);
                }


                int index = Random.Range(0, spawnPoints.Count);
                Transform pos = spawnPoints[index];

                enemy.transform.position = pos.position;
                enemy.SetActive(false);

                enemyCounter++;

                if(!bossRoom)
                    StartCoroutine(SpawnParticle(pos, enemy));

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

    IEnumerator SpawnParticle(Transform pos, GameObject enemy)
    {
        GameObject spawnEffect = PoolManager.Instance.SpawnFromPool("SpawnEffect") as GameObject;
        spawnEffect.transform.position = pos.position;
        yield return new WaitForSeconds(.6f);
        spawnEffect.SetActive(false);
        enemy.SetActive(true);
    }

    IEnumerator CheckForLastEnemy()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (enemyCounter <= 0)
        {
            StartCoroutine(LoadLastScene());
        }
    }

    IEnumerator LoadLastScene()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("EndGameWon");
    }
}
