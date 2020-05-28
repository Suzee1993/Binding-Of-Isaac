using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public enum EnemyState
{
    Wander,
    Follow,
    Attack,
    Die
};

public enum AttackType
{
    Shooter,
    Dasher,
    Bumper
};

public enum DeathType
{
    Exploder,
    NonExploder
};

public class Enemy : MonoBehaviour
{
    GameObject player;

    public EnemyState currentState = EnemyState.Wander;
    public AttackType attackType;
    public DeathType deathType;

    public float speed;
    public float range;
    public float attackRange;
    public float explosionRange;
    public float health;
    public string itemPrefab;

    private bool chooseDir = false;
    private Vector3 randomDir;
    //private bool dead = false;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case (EnemyState.Wander):
                Wander();
            break;

            case (EnemyState.Follow):
                Follow();
            break;

            case (EnemyState.Attack):
                Attack();
            break;

            case (EnemyState.Die):
                Die();
            break;
        };

        if (IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Wander;
        }

        if(IsPlayerInAttackRange(attackRange) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Attack;
        }
        else if (!IsPlayerInAttackRange(attackRange) && IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Follow;
        }
        else if (!IsPlayerInAttackRange(attackRange) && !IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }    
    private bool IsPlayerInAttackRange(float attackRange)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }

    public void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currentState = EnemyState.Follow;
        }
    }
    public void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
    }
    public void Attack()
    {
        if(attackType == AttackType.Dasher)
        {
            //TODO: Write Attack Pattern
        }
        else if(attackType == AttackType.Shooter)
        {
            //TODO: Write Attack Pattern
        }
        else if(attackType == AttackType.Bumper)
        {
            currentState = EnemyState.Follow;
        }
    }
    public void Die()
    {
        if (deathType == DeathType.Exploder)
        {
            StartCoroutine(Explode());
        }
        else if (deathType ==  DeathType.NonExploder)
        {
            //anim.SetTrigger("DeathCycle");
            DropItem();
        }

        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        //anim.SetTrigger("TakeDamage");
        health -= damage;
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    public GameObject DropItem()
    {
        int chance;

        if (deathType == DeathType.Exploder && attackType == AttackType.Shooter || deathType == DeathType.NonExploder && attackType == AttackType.Shooter)
        {
            //Droprate health item (for Shooter/Exploder) or damage item (for Shooter) 25%
            chance = Random.Range(0, 100);
            if(chance <= 25)
            {
                GameObject item = PoolManager.Instance.SpawnFromPool(itemPrefab);
                item.transform.position = transform.position;
                item.transform.rotation = transform.rotation;
                return item;
            }
            else
            {
                return null;
            }
        }
        else if (deathType == DeathType.NonExploder && attackType == AttackType.Dasher || deathType == DeathType.NonExploder && attackType == AttackType.Bumper)
        {
            //Droprate speed item (for Dasher) or attack speed item (for bumper) 40%
            chance = Random.Range(0, 100);
            Debug.Log(chance);
            if (chance <= 40)
            {
                Debug.Log("Item Dropped");
                GameObject item = PoolManager.Instance.SpawnFromPool(itemPrefab);
                item.transform.position = transform.position;
                item.transform.rotation = transform.rotation;
                return item;
            }
            else
            {
                Debug.Log("No Item Dropped");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true; 
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    private IEnumerator Explode()
    {
        //TODO: Exploding sequence
        DropItem();
        yield return new WaitForSeconds(1f);
    }
}
