using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(target))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
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
