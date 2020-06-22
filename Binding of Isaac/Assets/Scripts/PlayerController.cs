using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public float speed;
    public float bulletSpeed;
    public float damage;
    public static int collectedAmount = 0;

    [Header("Fire Delay Timer")]
    private float lastFire;
    public float fireDelay;

    [Header("SpriteRenderers")]
    public GameObject headSpriteRenderer;
    public GameObject bodySpriteRenderer;
    public Sprite hurtSprite;
    public Sprite deadSprite;

    public enum AnimationState
    {
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight,
        ShootUp,
        ShootDown,
        ShootLeft,
        ShootRight,
        Idle,
        Hurt,
        Dead
    };

    private AnimationState currentAnimationState = AnimationState.Idle;

    private LoadScreen ls;
    private Animator animHead;
    private Animator animBody;
    private SpriteRenderer sr;
    private SpriteRenderer srHead;
    private Rigidbody2D rb;
    private string stringName = "Bullet";
    private bool canMove =  true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ls = FindObjectOfType<LoadScreen>();
        sr = GetComponent<SpriteRenderer>();

        animHead = headSpriteRenderer.GetComponent<Animator>();
        animBody = bodySpriteRenderer.GetComponent<Animator>();
        srHead = headSpriteRenderer.GetComponent<SpriteRenderer>();

        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!ls.loadScreenActive && canMove)
        {
            damage = GameController.Damage;
            fireDelay = GameController.FireDelay;
            speed = GameController.Speed;

            //Movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            switch (currentAnimationState)
            {
                case (AnimationState.WalkUp):
                    WalkUp();
                    break;

                case (AnimationState.WalkDown):
                    WalkDown();
                    break;

                case (AnimationState.WalkLeft):
                    WalkLeft();
                    break;

                case (AnimationState.WalkRight):
                    WalkRight();
                    break;

                case (AnimationState.ShootUp):
                    ShootUp();
                    break;

                case (AnimationState.ShootDown):
                    ShootDown();
                    break;

                case (AnimationState.ShootLeft):
                    ShootLeft();
                    break;

                case (AnimationState.ShootRight):
                    ShootRight();
                    break;

                case (AnimationState.Idle):
                    Idle();
                    break;

                case (AnimationState.Hurt):
                    Hurt();
                    break;

                case (AnimationState.Dead):
                    Dead();
                    break;
            }


            #region Head & Body Walk Animations
            //Hurt Left
            if (horizontal < 0 && currentAnimationState == AnimationState.Hurt)
            {
                sr.flipX = true;
            }
            //Hurt Right
            if (horizontal > 0 && currentAnimationState == AnimationState.Hurt)
            {
                sr.flipX = false;
            }


            //Up
            if (vertical > 0 && currentAnimationState != AnimationState.Hurt || vertical > 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.WalkUp;
            }
            //Down
            if (vertical < 0 && currentAnimationState != AnimationState.Hurt || vertical < 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.WalkDown;
            }
            //Left
            if (horizontal < 0 && currentAnimationState != AnimationState.Hurt || horizontal < 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.WalkLeft;
            }
            //Right
            if (horizontal > 0 && currentAnimationState != AnimationState.Hurt || horizontal > 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.WalkRight;
            }
            #endregion

            rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);        

            //Shooting
            var shootHor = Input.GetAxis("ShootHor");
            var shootVer = Input.GetAxis("ShootVer");

            #region Head Shoot Animations
            //Up
            if (shootVer > 0 && currentAnimationState != AnimationState.Hurt || shootVer > 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.ShootUp;
            }
            //Down
            if (shootVer < 0 && currentAnimationState != AnimationState.Hurt || shootVer < 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.ShootDown;
            }
            //Left
            if (shootHor < 0 && currentAnimationState != AnimationState.Hurt || shootHor < 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.ShootLeft;
            }
            //Right
            if (shootHor > 0 && currentAnimationState != AnimationState.Hurt || shootHor > 0 && currentAnimationState != AnimationState.Dead)
            {
                currentAnimationState = AnimationState.ShootRight;
            }


            //Idle
            if (horizontal == 0 && vertical == 0 && shootHor == 0 && shootVer == 0 && currentAnimationState != AnimationState.Dead && currentAnimationState != AnimationState.Hurt)
            {
                currentAnimationState = AnimationState.Idle;
            }
            #endregion

            if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
            {
                Shoot(shootHor, shootVer);
                lastFire = Time.time;
            }

            //if(GameController.Health <= 0)
            //{
            //    StartCoroutine(LoadLastScene());
            //}

            if (GameController.hit)
            {
                currentAnimationState = AnimationState.Hurt;
                StartCoroutine(ResetHit());
            }

            if (GameController.dead)
            {
                currentAnimationState = AnimationState.Dead;
                canMove = false;
                StartCoroutine(LoadLastScene());
            }
        }
    }

    #region AnimationState Voids
    private void WalkUp()
    {
        animHead.SetTrigger("WalkUp");
    }

    private void WalkDown()
    {
        animHead.SetTrigger("WalkDown");
    }    
    
    private void WalkLeft()
    {
        srHead.flipX = true;
        animHead.SetTrigger("WalkLeft");
    }    
    
    private void WalkRight()
    {
        srHead.flipX = false;
        animHead.SetTrigger("WalkRight");
    }    
    
    private void ShootUp()
    {
        animHead.SetTrigger("ShootUp");
    }    
    
    private void ShootDown()
    {
        animHead.SetTrigger("ShootDown");
    }    
    
    private void ShootLeft()
    {
        srHead.flipX = true;
        animHead.SetTrigger("ShootLeft");
    }    
    
    private void ShootRight()
    {
        srHead.flipX = false;
        animHead.SetTrigger("ShootRight");
    }

    private void Hurt()
    {
        sr.sprite = hurtSprite;
        animHead.gameObject.SetActive(false);
        animBody.gameObject.SetActive(false);
    }

    private void Dead()
    {
        sr.sprite = deadSprite;
        animHead.gameObject.SetActive(false);
        animBody.gameObject.SetActive(false);
    }

    private void Idle()
    {
        animHead.SetTrigger("WalkDown");
        //animBody.SetBool("WalkDown", false);
        animBody.SetTrigger("BodyIdle");
    }
    #endregion

    void Shoot(float x, float y)
    {
        Debug.Log(damage);

        GameObject bullet = PoolManager.Instance.SpawnFromPool(stringName);
        bullet.GetComponent<BulletController>().damage = damage;
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            //An if statement within a vector3. 
            //The first part before : checks if x is less than 0, if so the bullet will go downwards. Floor = sets the value to an int. It goes down, or rather below 0, to -1
            //The : are the else part of this if statement
            //The second part after the : checks if x is greater than 0 and if so it sends the bullet upwards. Ceil = sets the value to an int. It goes up, or rather above 0, to +1
            //With this there will always ne a constant speed
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
    }

    static IEnumerator LoadLastScene()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Dead!");
        //SceneManager.LoadScene("EndGame");
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSecondsRealtime(3f);

        GameController.hit = false;
        sr.sprite = null;
        animHead.gameObject.SetActive(true);
        animBody.gameObject.SetActive(true);
    }

    #region Timer
    //public void StartTimer(float prevPlayerStat, float time, int state)
    //{
    //    StartCoroutine(Timer(prevPlayerStat, time, state));
    //}

    //IEnumerator Timer(float prevPlayerStat, float time, int stateOne)
    //{
    //    Debug.Log("Timer Start");
    //    yield return new WaitForSeconds(time);

    //    if (stateOne == damageItem) //1
    //    {
    //        damage = prevPlayerStat;
    //    }
    //    if (stateOne == speedItem) //2
    //    {
    //        speed = prevPlayerStat;
    //    }
    //    if (stateOne == attackSpeedItem) //3
    //    {
    //        fireDelay = prevPlayerStat;
    //    }
    //    Debug.Log("Timer End");
    //}
    #endregion
}
