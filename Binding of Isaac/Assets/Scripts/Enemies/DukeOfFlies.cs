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
    //public GameObject healthBarHolder;
    public Spawner spawner;

    private float fillValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        maxHealth = health;

        canvas.enabled = false;

        //healthBarHolder.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (spawner.playerInBossRoom)
        {
            currentState = EnemyState.Follow;
            canvas.enabled = true;
            //healthBarHolder.SetActive(true);
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
        //SpawnEternalFlies();
    }


    void SpawnEternalFlies()
    {

    }
}
