using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("General")]
    public float time;
    public int stateOne;
    public int stateTwo;

    [Header("State 1")]
    public float damageStat;
    [Header("State 2")]
    public float speedStat;
    [Header("State 3")]
    public float attackSpeedStat;
    [Header("State 4")]
    public float healthStat;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            ExecuteItem(player);
        }
    }

    protected virtual void ExecuteItem(PlayerController player)
    {
        gameObject.SetActive(false);
    }
}
