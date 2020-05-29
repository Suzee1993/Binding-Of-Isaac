using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float health;

    private Rigidbody2D rb;

    public Text collectedText;

    public static int collectedAmount = 0;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        collectedText.text = "Items Collected: " + collectedAmount;
        
    }

    public void TakeDamage(float damage)
    {
        //anim.SetTrigger("TakeDamage");
        health -= damage;
        if (health <= 0)
        {
            //TODO: Death function
        }
    }
}
