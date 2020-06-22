using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{    
    
    public enum MovementState
    {
        NULL,
        RL,
        UD
    };


    [Header("Charger Specific")]
    public float chargeSpeed = 5;
    public bool canDash = true;

    private Vector2 direction;
    private Rigidbody2D rb;

    Vector3 targetPos;
    public LayerMask mask;

    Vector2 rayR = Vector2.right;
    Vector2 rayL = Vector2.left;
    Vector2 rayU = Vector2.up;
    Vector2 rayD = Vector2.down;

    RaycastHit2D hitR;
    RaycastHit2D hitL;
    RaycastHit2D hitU;
    RaycastHit2D hitD;

    private MovementState movementState = MovementState.NULL;
    bool checkMove = true;

    public Spawner spawner;

    //Move randomly at the start
    protected override void OnEnable()
    {
        base.OnEnable();

        var dir = Random.Range(0, 360);
        randomDir = new Vector3(0, 0, dir);
        Quaternion rot = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rot, 1);
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if(currentState == EnemyState.Follow)
        {
            currentState = EnemyState.Wander;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Environment"))
        {
            currentState = EnemyState.Wander;

            StartCoroutine(ChooseDirection());
        }
    }

    protected override bool IsPlayerInAttackRange(float attackRange)
    {
        //return base.IsPlayerInAttackRange(attackRange);
        if (checkMove)
        {
            hitR = Physics2D.Raycast(transform.position, rayR, attackRange, mask);
            hitL = Physics2D.Raycast(transform.position, rayL, attackRange, mask);

            hitU = Physics2D.Raycast(transform.position, rayU, attackRange, mask);
            hitD = Physics2D.Raycast(transform.position, rayD, attackRange, mask);


            if(hitR)
            {
                movementState = MovementState.RL;
                checkMove = false;
                targetPos = hitR.point;
                return true;
            }
            if (hitL)
            {
                movementState = MovementState.RL;
                checkMove = false;
                targetPos = hitL.point;
                return true;
            }        
            if (hitU)
            {
                movementState = MovementState.UD;
                checkMove = false;
                targetPos = hitU.point;
                return true;
            }        
            if (hitD)
            {
                movementState = MovementState.UD;
                checkMove = false;
                targetPos = hitD.point;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected override void Wander()
    {
        //base.Wander();
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }
        transform.position += -transform.right * speed * Time.deltaTime;
    }

    protected override void Attack()
    {
        //base.Attack();
        if (canDash)
        {
            var target = targetPos;

            direction = (target - transform.position).normalized;

            if (movementState == MovementState.RL)
            {
                rb.velocity = new Vector2(direction.x, 0) * chargeSpeed;
                Debug.Log("Dash LR");

                if (canDash)
                {
                    StartCoroutine(DashReset());
                }
            }

            if (movementState == MovementState.UD)
            {
                rb.velocity = new Vector2(0, direction.y) * chargeSpeed;
                Debug.Log("Dash UD");

                if (canDash)
                {
                    StartCoroutine(DashReset());
                }
            }

        }
    }

    protected override void Die()
    {
        if (!dead)
        {
            dead = true;
            spawner.enemyCounter--;
        }
        base.Die();
    }

    IEnumerator DashReset()
    {        
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0) * 0;   
        currentState = EnemyState.Wander;    

        canDash = true;
        checkMove = true;
    }
}
