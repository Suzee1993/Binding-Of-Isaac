using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifetime;
    public float damage;
    public string target;

    private Animator anim;
    private Rigidbody2D rb;
    private bool canHit = true;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();



        StartCoroutine(Delay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(target) && canHit)
        {
            canHit = false;
            rb.velocity = Vector3.zero;
            if (target == "Player")
            {
                var player = other.GetComponent<PlayerController>();
                //player.TakeDamage(damage);
                GameController.TakeDamage(damage);


                anim.SetTrigger("DestroyBullet");
                StartCoroutine(WaitForAnim());


            }
            else if (target == "Enemy" && !this.name.Contains("Blood"))
            {
                rb.velocity = Vector3.zero;

                if (other.name.Contains("Eternal"))
                {
                    var fly = other.GetComponent<EternalFly>();
                    fly.TakeDamage(damage);



                    anim.SetTrigger("DestroyBullet");
                    StartCoroutine(WaitForAnim());
                }
                else
                {
                    var enemy = other.GetComponent<Enemy>();
                    enemy.Damage(damage);



                    anim.SetTrigger("DestroyBullet");
                    StartCoroutine(WaitForAnim());
                }

            }
            //else if (target == "Fly")
            //{
            //    var fly = other.GetComponent<EternalFly>();
            //    fly.TakeDamage(damage);
            //    gameObject.SetActive(false);
            //}
        }
        else if (other.CompareTag("Environment") || other.CompareTag("Collectable") && this.name.Contains("Tear"))
        {
            Debug.Log(this.name + other.name);

            //gameObject.SetActive(false);
            rb.velocity = Vector3.zero;
            anim.SetBool("DestroyBullet", true);
            StartCoroutine(WaitForAnim());
        }
        else if(other.CompareTag("Collectable") && this.name.Contains("Blood"))
        {
            return;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(lifetime);
        rb.velocity = Vector3.zero;
        anim.SetBool("DestroyBullet", true);
        StartCoroutine(WaitForAnim());

        //this.gameObject.SetActive(false);
    }

    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(0.6f);
        anim.SetBool("DestroyBullet", false);
        gameObject.SetActive(false);
    }
}
