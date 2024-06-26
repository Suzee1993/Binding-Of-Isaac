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
    public GameObject spriteHolder;

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
    private Animator anim;

    private Rigidbody2D rb;

    private enum AnimationState
    {
        Idle,
        Attack
    };

    AnimationState currentAnimationState = AnimationState.Idle;

    protected override void OnEnable()
    {
        base.OnEnable();
        maxHealth = health;

        anim = GetComponentInChildren<Animator>();

        canvas.enabled = false;

        randomDFTime = 4f;
        randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);
        randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);

        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        //base.Update();
        if (!dead)
        {
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

            switch (currentAnimationState)
            {
                case (AnimationState.Idle):
                    AnimationIdle();
                    break;

                case (AnimationState.Attack):
                    AnimationAttack();
                    break;
            }

            if (spawner.playerInBossRoom)
            {
                currentState = EnemyState.Attack;

                StartCoroutine(ActivateHealthBar());
                //canvas.enabled = true;
            }
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
        currentAnimationState = AnimationState.Idle;

        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += -transform.right * speed * Time.deltaTime;
    }

    protected override void Die()
    {
        if (!dead)
        {
            dead = true;
            foreach (EternalFly e in attackFliesList)
            {
                e.bossIsAlive = false;
            }
            foreach (EternalFly e in defendFliesList)
            {
                e.bossIsAlive = false;
            }

            rb.velocity = Vector3.zero;

            StartCoroutine(LastAttack());

            spawner.enemyCounter--;

            GameObject explosion = PoolManager.Instance.SpawnFromPool("SpawnEffect") as GameObject;
            explosion.transform.position = transform.position;
            explosion.transform.localScale = new Vector3(.3f, .3f, 0);

            spriteHolder.SetActive(false);

            StartCoroutine(DeathScene(explosion));
        }
    }

    void SpawnDefendFlies()
    {
        currentAnimationState = AnimationState.Attack;

        int amount = Random.Range(minAmountFlies, maxAmountFlies);

        StartCoroutine(DelayedDefendFlySpawn(amount));
    }

    void SpawnSmallAttackFlies()
    {
        currentAnimationState = AnimationState.Attack;
        int amount = Random.Range(minAmountFlies, maxAmountFlies);


        StartCoroutine(AnimationDelay(amount));

        attackFliesTimer = 0;
        randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
        isSpawning = false;
    }

    IEnumerator ActivateHealthBar()
    {
        yield return new WaitForSecondsRealtime(3.5f);

        canvas.enabled = true;
    }

    IEnumerator AnimationDelay(int amount)
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(DelayedSmallAttackFlySpawn(amount));
    }

    IEnumerator DelayedSmallAttackFlySpawn(int amount)
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

        currentAnimationState = AnimationState.Idle;
    }

    IEnumerator DelayedDefendFlySpawn(int amount)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < amount; i++)
        {
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("DefendEternalFly") as GameObject;

            //GameController.instance.enemyKillCounter++;

            FlyInfo(eternalFly);

            efScript.index = Random.Range(0, defendPoints.Count);

            spawner.enemyCounter++;
        }

        currentAnimationState = AnimationState.Idle;
    }

    void SpawnBigAttackFly()
    {
        currentAnimationState = AnimationState.Attack;

        StartCoroutine(DelayerBigAttackFlySpawn());
    }

    IEnumerator DelayerBigAttackFlySpawn()
    {
        yield return new WaitForSeconds(1f);

        GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;

        FlyInfo(eternalFly);

        efScript.flyType = FlyType.Attack;
        eternalFly.transform.localScale += new Vector3(1.5f, 1.5f, 0);
        spawner.enemyCounter++;

        currentAnimationState = AnimationState.Idle;
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

    void AnimationIdle()
    {
        anim.SetTrigger("Idle");
    }

    void AnimationAttack()
    {
        anim.SetTrigger("Attack");
    }


    IEnumerator LastAttack()
    {
        int amount = Random.Range(minAmountFlies, maxAmountFlies);

        for (int i = 0; i < amount; i++)
        {
            GameObject eternalFly = PoolManager.Instance.SpawnFromPool("AttackEternalFly") as GameObject;

            FlyInfo(eternalFly);

            efScript.speed = 0.6f;
            spawner.enemyCounter++;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator DeathScene(GameObject explosion)
    {
        yield return new WaitForSecondsRealtime(1.2f);
        explosion.SetActive(false);
        spriteHolder.SetActive(true);
        gameObject.SetActive(false);
    }
}
