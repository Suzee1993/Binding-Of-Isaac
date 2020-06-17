using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyState
{
    Defend,
    Attack,
    Die,
};

public enum FlyType
{
    Defend,
    Attack
}

public class EternalFly : MonoBehaviour
{
    public FlyState flyState;
    public FlyType flyType;

    public float speed;
    public float health = 2f;
    public float damage = 1f;
    public float attackRange;
    public bool bossIsAlive = true;


    public DukeOfFlies boss;
    public Spawner spawner;
    private PlayerController player;
    private bool defendPointReached = false;

    public bool coolDownAttack = false;
    public float coolDownTime;

    public int index;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        switch (flyState)
        {
            case (FlyState.Defend):
                    Defend(index);
            break;    

            case (FlyState.Attack):
                    Attack();
            break;

            case (FlyState.Die):
                    Die();
            break;
        };

        if (!bossIsAlive)
            flyType = FlyType.Attack;


        if(flyType == FlyType.Defend && flyState != FlyState.Die)
        {
            flyState = FlyState.Defend;
        }

        if(flyType == FlyType.Attack && flyState != FlyState.Die)
        {
            flyState = FlyState.Attack;
        }
    }

    private void Defend(int index)
    {
        transform.position = Vector2.MoveTowards(transform.position, boss.defendPoints[index].position, speed * Time.deltaTime);

        //if (flyType == FlyType.Defend && !defendPointReached)
        //{
        //    int index = Random.Range(0, boss.defendPoints.Count);

        //    transform.position = Vector2.MoveTowards(transform.position, boss.defendPoints[index].position, speed * Time.deltaTime);

        //    if (transform.position == boss.defendPoints[index].position)
        //    {
        //        defendPointReached = true;
        //    }
        //    else if(transform.position != boss.defendPoints[index].position)
        //    {
        //        defendPointReached = false ;
        //    }
        //}
            
    } 
    
    private void Attack()
    {
        if(flyType == FlyType.Attack)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!coolDownAttack && collision.CompareTag("Player"))
        {
            GameController.TakeDamage(damage);

            StartCoroutine(CoolDown());
        }
    }

    protected virtual void Die()
    {
        //anim.SetTrigger("DeathCycle");
        spawner.enemyCounter--;
        //GameController.instance.enemyKillCounter--;

        if (flyType == FlyType.Defend)
            boss.defendFliesList.Remove(this);

        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            flyState = FlyState.Die;
        }
    }

    protected virtual IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDownTime);
        coolDownAttack = false;
    }
}
