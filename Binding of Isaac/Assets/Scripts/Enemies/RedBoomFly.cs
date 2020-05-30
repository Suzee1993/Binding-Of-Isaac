using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

public class RedBoomFly : Enemy
{
    [Header("RedBoomFly Specific")]
    public string bulletName;
    public float bulletSpeed = 3;
    public float radius = 2;

    int[] directions = { 45, -45, 135, -135 };
    Vector2 start;
    int numberOfBullets = 6;

    //Only moves diagonally
    protected override void OnEnable()
    {
        base.OnEnable();

        var dir = Random.Range(0, directions.Length);
        var direc = directions[dir];
        randomDir = new Vector3(0, 0, direc);
        Quaternion rot = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rot, 1);
    }

    protected override IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        var dir = Random.Range(0, directions.Length);
        var direc = directions[dir];
        Debug.Log(direc);
        randomDir = new Vector3(0, 0, direc);
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.localRotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = EnemyState.Die;
        }
    }

    protected override void Die()
    {
        StartCoroutine(Explode());
        //base.Die();
    }

    private IEnumerator Explode()
    {
        start = new Vector2(transform.position.x, transform.position.y);
        float angleStep = 360f / numberOfBullets;
        float angle = 0f;

        for (int i = 0; i <= numberOfBullets; i++)
        {
            float bulletDirXPos = transform.position.x + Mathf.Sin((angle * math.PI) / 180) * radius;
            float bulletDirYPos = transform.position.y + Mathf.Cos((angle * math.PI) / 180) * radius;

            Vector2 bulletVector = new Vector2(bulletDirXPos, bulletDirYPos);
            Vector2 bulletMoveDir = (bulletVector - start).normalized * bulletSpeed;

            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletName);
            bullet.transform.position = start;

            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDirXPos, bulletDirYPos);

            angle += angleStep;
        }

        DropItem();
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
}
