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

    private Animator anim;
    private SpriteRenderer sr;

    private enum AnimationState
    {
        WanderUp,
        WanderDown,
        WanderLeft,
        WanderRight,
        AttackUp,
        AttackDown,
        AttackLeft,
        AttackRight,
        Null
    };

    private AnimationState currentAnimationState = AnimationState.Null;

    //Move randomly at the start
    protected override void OnEnable()
    {
        movementState = MovementState.NULL;

        base.OnEnable();

        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        var dir = Random.Range(0, 360);

        CheckDir(dir);

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

        switch (currentAnimationState)
        {
            case (AnimationState.WanderUp):
                WanderUp();
                break;

            case (AnimationState.WanderDown):
                WanderDown();
                break;

            case (AnimationState.WanderLeft):
                WanderLeft();
                break;

            case (AnimationState.WanderRight):
                WanderRight();
                break;

            case (AnimationState.AttackUp):
                AttackUp();
                break;

            case (AnimationState.AttackDown):
                AttackDown();
                break;

            case (AnimationState.AttackLeft):
                AttackLeft();
                break;

            case (AnimationState.AttackRight):
                AttackRight();
                break;

            case (AnimationState.Null):
                Idle();
                break;
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
                currentAnimationState = AnimationState.AttackRight;
                checkMove = false;
                targetPos = hitR.point;
                return true;
            }
            if (hitL)
            {
                movementState = MovementState.RL;
                currentAnimationState = AnimationState.AttackLeft;
                checkMove = false;
                targetPos = hitL.point;
                return true;
            }        
            if (hitU)
            {
                movementState = MovementState.UD;
                currentAnimationState = AnimationState.AttackUp;
                checkMove = false;
                targetPos = hitU.point;
                return true;
            }        
            if (hitD)
            {
                movementState = MovementState.UD;
                currentAnimationState = AnimationState.AttackDown;
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
                rb.velocity = new Vector2(direction.x, 0).normalized * chargeSpeed;
                Debug.Log("Dash LR");

                if (canDash)
                {
                    StartCoroutine(DashReset());
                }
            }

            if (movementState == MovementState.UD)
            {
                rb.velocity = new Vector2(0, direction.y).normalized * chargeSpeed;
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

    protected override IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        int dir = Random.Range(0, 360);

        CheckDir(dir);

        randomDir = new Vector3(0, 0, dir);
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(.5f, 3f));
        chooseDir = false;
    }

    #region Animation State Voids
    private void WanderUp()
    {
        anim.SetTrigger("WanderUp");
    }

    private void WanderDown()
    {
        anim.SetTrigger("WanderDown");
    }

    private void WanderLeft()
    {
        sr.flipX = true;
        anim.SetTrigger("WanderLeft");
    }

    private void WanderRight()
    {
        sr.flipX = false;
        anim.SetTrigger("WanderRight");
    }

    private void AttackUp()
    {
        anim.SetTrigger("AttackUp");
    }

    private void AttackDown()
    {
        anim.SetTrigger("AttackDown");
    }

    private void AttackLeft()
    {
        sr.flipX = true;
        anim.SetTrigger("AttackLeft");
    }

    private void AttackRight()
    {
        sr.flipX = false;
        anim.SetTrigger("AttackRight");
    }

    private void Idle()
    {
        anim.SetTrigger("NoInput");
    }

    #endregion

    IEnumerator DashReset()
    {        
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0) * 0;   
        currentState = EnemyState.Wander;    

        canDash = true;
        checkMove = true;
    }

    void CheckDir(int dir)
    {
        if (dir <= 45 || dir >= 316)
        {
            //up
            Debug.Log("Up" + dir);
            currentAnimationState = AnimationState.WanderUp;
        }
        if (dir >= 46 && dir <= 135)
        {
            //right
            Debug.Log("Right" + dir);
            currentAnimationState = AnimationState.WanderRight;
        }
        if (dir >= 136 && dir <= 225)
        {
            //down
            Debug.Log("Down" + dir);
            currentAnimationState = AnimationState.WanderDown;
        }
        if (dir >= 226 && dir <= 315)
        {
            //left
            Debug.Log("Left" + dir);
            currentAnimationState = AnimationState.WanderLeft;
        }
    }
}
