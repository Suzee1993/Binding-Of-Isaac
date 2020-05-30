using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    [Header("Charger Specific")]
    public float chargeSpeed = 5;
    public bool canDash = true;

    private Vector2 direction;
    private float dashTime = 0f;

    //Move randomly at the start
    protected override void OnEnable()
    {
        base.OnEnable();

        var dir = Random.Range(0, 360);
        randomDir = new Vector3(0, 0, dir);
        Quaternion rot = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rot, 1);
    }
    protected override void Attack()
    {
        //base.Attack();
        if (dashTime < 1f && canDash)
        {
            dashTime += Time.deltaTime;

            var target = player;

            direction = (target.transform.position - transform.position).normalized * chargeSpeed;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
        }
        
        if(dashTime > 1f && canDash)
        {
            canDash = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            currentState = EnemyState.Wander;
            StartCoroutine(DashReset());
        }
    }
    IEnumerator DashReset()
    {
        currentState = EnemyState.Wander;
        yield return new WaitForSeconds(2f);
        dashTime = 0;
        canDash = true;
    }
}
