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

    public bool coolDownAttack = false;
    public float coolDownTime;

    public int index;

    private Animator anim;
    private bool died = false;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();
        //anim = GetComponentInChildren<Animator>();

        //anim.SetBool("Die", false);
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


        if (flyType == FlyType.Defend && flyState != FlyState.Die)
        {
            flyState = FlyState.Defend;
        }

        if (flyType == FlyType.Attack && flyState != FlyState.Die)
        {
            flyState = FlyState.Attack;
        }
    }

    private void Defend(int index)
    {
        transform.position = Vector2.MoveTowards(transform.position, boss.defendPoints[index].position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        if (flyType == FlyType.Attack)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (!coolDownAttack && collision.CompareTag("Player"))
        {
            GameController.TakeDamage(damage);

            StartCoroutine(CoolDown());
        }
    }

    private void Die()
    {
        if (!died)
        {
            died = true;
            //anim.SetTrigger("DeathCycle");
            spawner.enemyCounter--;
            //GameController.instance.enemyKillCounter--;


            if (flyType == FlyType.Defend)
                boss.defendFliesList.Remove(this);

            if (flyType == FlyType.Attack)
                boss.attackFliesList.Remove(this);

            //anim.SetBool("Die", true);

            StartCoroutine(WaitToDie());
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            flyState = FlyState.Die;
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDownTime);
        coolDownAttack = false;
    }

    private IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(0.6f);

        gameObject.SetActive(false);
    }
}
