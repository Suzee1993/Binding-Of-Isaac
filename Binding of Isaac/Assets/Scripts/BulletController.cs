using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifetime;
    public float damage;
    public string target;

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(target))
        {
            if(target == "Player")
            {
                var player = other.GetComponent<PlayerController>();
                //player.TakeDamage(damage);
                GameController.TakeDamage(damage);
                gameObject.SetActive(false);
            }
            else if (target == "Enemy")
            {
                var enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }        

        //else if(other.CompareTag("Environment") || other.CompareTag("Collectable"))
        //{
           // this.gameObject.SetActive(false);
        //}
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(lifetime);
        this.gameObject.SetActive(false);
    }
}
