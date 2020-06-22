﻿using System.Collections;
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
    public List<EternalFly> attackFliesList = new List<EternalFly>();
    public int minAmountFlies;
    public int maxAmountFlies;
    public List<Transform> defendPoints = new List<Transform>();

    private EternalFly efScript;
    private float fillValue;



    //DefendFliesTimer
    private float randomDFTime;
    private float randomDFTimeMin = 8f;
    private float randomDFTimeMax = 12f;
    private float defendFliesTimer = 0;

    //BigAttackFlyTimer
    private float randomBAFTime;
    private float randomBAFTimeMin = 15f;
    private float randomBAFTimeMax = 20f;
    private float bigAttackFlyTimer = 0;

    //SmallAttackFliesTimer
    private float randomAFTime;
    private float randomAFTimeMin = 1;
    private float randomAFTimeMax = 1.5f;
    private float attackFliesTimer = 0;

    private bool isSpawning = false;
    private bool died = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        maxHealth = health;

        canvas.enabled = false;

        randomDFTime = 4f;
        randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);
        randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);

        //StartCoroutine(Wait());
    }

    protected override void Update()
    {
        //base.Update();

        switch (currentState)
        {
            case (EnemyState.Wander):
                Wander();
                break;

            case (EnemyState.Attack):
                Attack();
                break;

            case (EnemyState.Die):
                Die();
                break;
        };

        if (spawner.playerInBossRoom)
        {
            currentState = EnemyState.Attack;

            StartCoroutine(ActivateHealthBar());
            //canvas.enabled = true;
        }
    }

    protected override void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            if (!died)
            {
                died = true;
                currentState = EnemyState.Die;
            }

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
        bigAttackFlyTimer += Time.deltaTime;
        //randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
        attackFliesTimer += Time.deltaTime;

        //Wander
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += -transform.right * speed * Time.deltaTime;

        //DEFENDFLIES
        if (defendFliesList.Count == 0)
        {
            defendFliesTimer += Time.deltaTime;

            if(defendFliesTimer >= randomDFTime)
            {
                defendFliesTimer = 0;
                randomDFTime = Random.Range(randomDFTimeMin, randomDFTimeMax);

                SpawnDefendFlies();
            }
        }

        //SMALLATTACKFLIES
        if (defendFliesList.Count != 0 && attackFliesTimer >= randomAFTime && attackFliesList.Count == 0 && !isSpawning)
        {
            isSpawning = true;
            SpawnSmallAttackFlies();
        }

        //BIGATTACKFLY
        if (bigAttackFlyTimer >= randomBAFTime && attackFliesList.Count == 0)
        {
            bigAttackFlyTimer = 0;
            randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);

            SpawnBigAttackFly();
        }

        //RESETTIMERS
        if(bigAttackFlyTimer > 60f)
        {
            bigAttackFlyTimer = 0;
            randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);
        }
        else if(attackFliesTimer > 60f)
        {
            attackFliesTimer = 0;
            randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
        }
    }

    protected override void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += -transform.right * speed * Time.deltaTime;
    }

    protected override void Die()
    {
        efScript.bossIsAlive = false;
        spawner.enemyCounter--;
        //GameController.instance.enemyKillCounter--;
        base.Die();
    }

    void SpawnDefendFlies()
    {
        int amount = Random.Range(minAmountFlies, maxAmountFlies);

        for (int i = 0; i < amount; i++)
        {
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("DefendEternalFly") as GameObject;

            //GameController.instance.enemyKillCounter++;

            FlyInfo(eternalFly);

            efScript.index = Random.Range(0, defendPoints.Count);

            spawner.enemyCounter++;
        }

    }

    void SpawnSmallAttackFlies()
    {
        int amount = Random.Range(minAmountFlies, maxAmountFlies);        

        StartCoroutine(DelayedSpawn(amount));

        attackFliesTimer = 0;
        randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
        isSpawning = false;
    }

    IEnumerator ActivateHealthBar()
    {
        yield return new WaitForSecondsRealtime(3.5f);

        canvas.enabled = true;
    }

    IEnumerator DelayedSpawn(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;


            //GameController.instance.enemyKillCounter++;

            FlyInfo(eternalFly);

            efScript.speed = 0.6f;
            spawner.enemyCounter++;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SpawnBigAttackFly()
    {
        GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;


        //GameController.instance.enemyKillCounter++;

        FlyInfo(eternalFly);

        efScript.flyType = FlyType.Attack;
        eternalFly.transform.localScale += new Vector3(1.5f, 1.5f, 0);
        spawner.enemyCounter++;
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
        else if(efScript.flyType == FlyType.Attack)
        {
            attackFliesList.Add(efScript);
        }

        eternalFly.transform.position = spawnPoint.position;
        eternalFly.transform.rotation = spawnPoint.transform.rotation;
    }
}
