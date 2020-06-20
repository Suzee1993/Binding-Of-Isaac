using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Horf : Enemy
{
    [Header("Horf Specific")]
    public string bulletName;
    public float bulletSpeed;
    public GameObject shootPoint;

    private Vector2 direction;
    private float lastFire;
    public float fireDelay;

    public Spawner spawner;

    private Animator anim;

    protected override void OnEnable()
    {
        base.OnEnable();
        anim = GetComponentInChildren<Animator>();

        anim.SetBool("Die", false);
    }

    //Doesn't move at all
    protected override void Wander()
    {
        //base.Wander();
    }
    protected override void Attack()
    {
        if(Time.time > lastFire + fireDelay)
        {
            anim.SetBool("Attack", true);

            //base.Attack();
            var target = player;

            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletName);
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;

            direction = (target.transform.position - transform.position).normalized * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
            lastFire = Time.time;
            StartCoroutine(ResetAnimBool());
        }


    }

    IEnumerator ResetAnimBool()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool("Attack", false);
    }

    protected override void Die()
    {
        spawner.enemyCounter--;
        anim.SetBool("Die", true);
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(.5f);
        base.Die();
    }
}
