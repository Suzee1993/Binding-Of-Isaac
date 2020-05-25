using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float speed;

    private float lastFire;
    public float fireDelay;

    private string stringName = "Bullet";

    void Start()
    {
        
    }

    void Update()
    {
        var shootHor = Input.GetAxis("ShootHor");
        var shootVer = Input.GetAxis("ShootVer");

        if((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = PoolManager.Instance.SpawnFromPool(stringName);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * speed : Mathf.Ceil(x) * speed,
            //An if statement within a vector3. 
            //The first part before : checks if x is less than 0, if so the bullet will go downwards. Floor = sets the value to an int. It goes down, or rather below 0, to -1
            //The : are the else part of this if statement
            //The second part after the : checks if x is greater than 0 and if so it sends the bullet upwards. Ceil = sets the value to an int. It goes up, or rather above 0, to +1
            //With this there will always ne a constant speed
            (y < 0) ? Mathf.Floor(y) * speed : Mathf.Ceil(y) * speed,
            0
        );

    }
}
