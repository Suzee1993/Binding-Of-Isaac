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



    //DefendFliesTimer
    private float randomDFTime;
    private float randomDFTimeMin = 8f;
    private float randomDFTimeMax = 12f;
    private float defendFliesTimer = 0;

    //BigAttackFlyTimer
    private float randomBAFTime;
    private float randomBAFTimeMin = 15f;
    private float randomBAFTimeMax = 20f;
    private float BigAttackFlyTimer = 0;

    //SmallAttackFliesTimer
    private float randomAFTime;
    private float randomAFTimeMin = 1;
    private float randomAFTimeMax = 1.5f;
    private float attackFliesTimer = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        maxHealth = health;

        canvas.enabled = false;

        randomDFTime = 4f;
        randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);

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
        BigAttackFlyTimer = +Time.deltaTime;

        //base.Attack();

        currentState = EnemyState.Wander;

        if(defendFliesList.Count == 0)
        {
            defendFliesTimer += Time.deltaTime;

            if(defendFliesTimer == randomDFTime)
            {
                defendFliesTimer = 0;
                randomDFTime = Random.Range(randomDFTimeMin, randomDFTimeMax);

                SpawnDefendFlies();
            }
        }
        


        if(BigAttackFlyTimer == randomBAFTime)
        {
            BigAttackFlyTimer = 0;
            randomBAFTime = Random.Range(randomBAFTimeMin, randomBAFTimeMax);

            //SpawnBigAttackFly();
        }
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
            efScript.speed = 0.8f;

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Environment"))
        {
            StartCoroutine(ChooseDirection());
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

        efScript.flyType = FlyType.Attack;
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

            efScript.index = Random.Range(0, defendPoints.Count);

            //StartCoroutine(MoveDefendFly());

            //eternalFly.transform.position = spawnPoint.position;
            //eternalFly.transform.rotation = spawnPoint.transform.rotation;
        }

        randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
        attackFliesTimer += Time.deltaTime;


        //TODO: Build Timer and Randomizer
        if (defendFliesList.Count != 0 && attackFliesTimer == randomDFTime)
        {
            SpawnSmallAttackFlies();
        }
        else
        {
            attackFliesTimer = 0;
            randomAFTime = Random.Range(randomAFTimeMin, randomAFTimeMax);
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

    //IEnumerator MoveDefendFly()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    int index = Random.Range(0, defendPoints.Count);
    //}
}
