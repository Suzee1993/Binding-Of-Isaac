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

    private LoadScreen ls;
    private Animator anim;
    private Rigidbody2D rb;
    private string stringName = "Bullet";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ls = FindObjectOfType<LoadScreen>();
        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!ls.loadScreenActive)
        {
            damage = GameController.Damage;
            fireDelay = GameController.FireDelay;
            speed = GameController.Speed;

            //Movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        

            //Shooting
            var shootHor = Input.GetAxis("ShootHor");
            var shootVer = Input.GetAxis("ShootVer");

            if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
            {
                Shoot(shootHor, shootVer);
                lastFire = Time.time;
            }

            if(GameController.Health <= 0)
            {
                StartCoroutine(LoadLastScene());
            }
        }
    }

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

        SceneManager.LoadScene("EndScene");
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
