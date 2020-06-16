using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DukeOfFlies : Enemy
{
    [Header("Duke of Flies specific")]
    public Canvas canvas;
    public Image healthBarFilled;
    public Text healthText;
    public float maxHealth;
    public Spawner spawner;
    public Transform spawnPoint;
    public List<EternalFly> defendFliesList = new List<EternalFly>();
    public int minAmountFlies;
    public int maxAmountFlies;
    public List<Transform> defendPoints = new List<Transform>();

    private EternalFly efScript;
    private float fillValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        maxHealth = health;

        canvas.enabled = false;

        StartCoroutine(Wait());
    }

    protected override void Update()
    {
        base.Update();

        if (spawner.playerInBossRoom)
        {
            currentState = EnemyState.Follow;
            canvas.enabled = true;
        }
    }

    protected override void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    void UpdateHealthBar()
    {
        fillValue = health;
        fillValue = fillValue / maxHealth;

        healthBarFilled.fillAmount = fillValue;
    }

    protected override void Attack()
    {
        base.Attack();
        //TODO: Build timer
        SpawnDefendFlies();
        //SpawnBigAttackFly();


    }

    protected override void Die()
    {
        efScript.bossIsAlive = false;
        base.Die();
    }


    void SpawnSmallAttackFlies()
    {
        int amount = Random.Range(minAmountFlies, maxAmountFlies);        
  
        for (int i = 0; i < amount; i++)
        {
            //TODO: Amount of flies
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;

            FlyInfo(eternalFly);

            efScript.flyType = FlyType.Attack;

        }

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        SpawnDefendFlies();
    }

    void SpawnBigAttackFly()
    {
        GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;

        FlyInfo(eternalFly);

        efScript.flyType = FlyType.Defend;
        eternalFly.transform.localScale += new Vector3(0.5f, 0.5f, 0);

        //eternalFly.transform.position = spawnPoint.position;
        //eternalFly.transform.rotation = spawnPoint.transform.rotation;
    }

    void SpawnDefendFlies()
    {
        int amount = Random.Range(minAmountFlies, maxAmountFlies);

        for (int i = 0; i < amount; i++)
        {
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("DefendEternalFly") as GameObject;

            //efScript = eternalFly.GetComponent<EternalFly>();
            //efScript.boss = this;
            //efScript.spawner = spawner;
            //defendFliesList.Add(efScript);

            FlyInfo(eternalFly);

            efScript.flyType = FlyType.Defend;
            //eternalFly.transform.position = spawnPoint.position;
            //eternalFly.transform.rotation = spawnPoint.transform.rotation;
        }

        //TODO: Build Timer and Randomizer
        if (defendFliesList.Count != 0)
        {
            SpawnSmallAttackFlies();
        }
    }

    void FlyInfo(GameObject eternalFly)
    {
        efScript = eternalFly.GetComponent<EternalFly>();
        efScript.boss = this;
        efScript.spawner = spawner;

        if(efScript.flyType == FlyType.Defend)
        {
            defendFliesList.Add(efScript);
        }

        eternalFly.transform.position = spawnPoint.position;
        eternalFly.transform.rotation = spawnPoint.transform.rotation;
    }
}
