using System.Collections;
using System.Collections.Generic;
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

    protected override void OnEnable()
    {
        base.OnEnable();
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
            //base.Attack();
            var target = player;

            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletName);
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;

            direction = (target.transform.position - transform.position).normalized * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
            lastFire = Time.time;
        }
    }

    protected override void Die()
    {
        spawner.enemyCounter--;
        base.Die();
    }
}
