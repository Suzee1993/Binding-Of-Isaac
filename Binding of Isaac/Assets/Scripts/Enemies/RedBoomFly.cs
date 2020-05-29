using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBoomFly : Enemy
{
    int[] directions = { 45 , -45, 135, -135};

    //Only moves diagonally
    protected override void Start()
    {
        base.Start();

        var dir = Random.Range(0, directions.Length);
        var direc = directions[dir];
        Debug.Log(direc);
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
            //Explode, instantiate red bombs
        }
    }

    protected override void Die()
    {
        StartCoroutine(Explode());
        base.Die();
    }

    private IEnumerator Explode()
    {
        //TODO: Exploding sequence
        //Instantiate 6 bullets in different directions
        DropItem();
        yield return new WaitForSeconds(1f);
    }
}
