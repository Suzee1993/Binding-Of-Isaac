﻿using System;
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
    Die,
    Null
};

public class Enemy : MonoBehaviour
{
    [Header ("Don't Touch")]
    public GameObject player;
    public string itemPrefab;

    [Header("General")]
    public EnemyState currentState = EnemyState.Wander;
    public float speed;
    public float range;
    public float damage;
    public float attackRange;
    public float health;
    public List<string> itemNames = new List<string>();
    public int chanceRate;

    protected bool chooseDir = false;
    protected Vector3 randomDir;
    protected bool dead = false;

    public bool coolDownAttack = false;
    public float coolDownTime;

    protected virtual void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
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

    protected virtual bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }
    protected virtual bool IsPlayerInAttackRange(float attackRange)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }

    protected virtual void Wander()
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
    protected virtual void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
    }
    //Differs per enemy
    protected virtual void Attack()
    {
        currentState = EnemyState.Follow;
    }
    protected virtual void Die()
    {
        DropItem();
        gameObject.SetActive(false);
    }

    public void Damage(float damage)
    {
        TakeDamage(damage);
    }

    protected virtual void TakeDamage(float damage)
    {
        //anim.SetTrigger("TakeDamage");
        health -= damage;
        if (health <= 0)
        {
            currentState = EnemyState.Die;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!coolDownAttack && collision.CompareTag("Player"))
        {
            GameController.TakeDamage(damage);

            StartCoroutine(CoolDown());
        }
    }
    
    protected virtual GameObject DropItem()
    {
        var randItem = Random.Range(0, itemNames.Count);
        itemPrefab = itemNames[randItem];

        int chance;
        chance = Random.Range(0, 100);
        Debug.Log(chance);

        if (chance <= chanceRate)
        {
            Debug.Log("Item Dropped");
            GameObject item = PoolManager.Instance.SpawnFromPool(itemPrefab);
            item.transform.position = transform.position;
            item.transform.rotation = Quaternion.identity;
            return item;
        }
        else
        {
            Debug.Log("No Item Dropped");
            return null;
        }


    }

    protected virtual IEnumerator ChooseDirection()
    {
        chooseDir = true; 
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    protected virtual IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDownTime);
        coolDownAttack = false;
    }



}
