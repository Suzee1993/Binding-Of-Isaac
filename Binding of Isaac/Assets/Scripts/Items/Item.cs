﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("General")]
    public float time;
    public int stateOne;
    public int stateTwo;

    public enum ItemTypes
    {
        Null,
        Health,
        Speed,
        AttackSpeed,
        Damage
    };
    public ItemTypes itemType;

    [Header("State 1")]
    public float damageStat;
    [Header("State 2")]
    public float speedStat;
    [Header("State 3")]
    public float attackSpeedStat;
    [Header("State 4")]
    public float healthStat;

    [Header("Inventory Info")]
    public string title;
    public string description;
    public Sprite sprite;

    protected Inventory inventory;

    protected virtual void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddToInventory();
            //var player = collision.gameObject.GetComponent<PlayerController>();
            //ExecuteItem();// player);
        }
    }
    protected virtual void AddToInventory()
    {        
        gameObject.SetActive(false);

        inventory.AddToInventory(sprite, title, description, itemType, damageStat, speedStat, attackSpeedStat, healthStat);

    }

    //protected virtual void ExecuteItem()//PlayerController player)
    //{
    //    if(itemType == ItemTypes.Health)
    //    {
    //        GameController.Heal(healthStat);
    //        inventory.RevomeFromInventory();
    //    }
    //    else if (itemType == ItemTypes.Speed)
    //    {
    //        GameController.SpeedUp(speedStat, damageStat, time, itemType); 
    //        inventory.RevomeFromInventory();
    //    }
    //    else if (itemType == ItemTypes.AttackSpeed)
    //    {
    //        GameController.AttackSpeedUp(attackSpeedStat, speedStat, time, itemType);
    //        inventory.RevomeFromInventory();
    //    }
    //    else if (itemType == ItemTypes.Damage)
    //    {
    //        GameController.DamageUp(damageStat,time, itemType);
    //        inventory.RevomeFromInventory();
    //    }

    //    gameObject.SetActive(false);
    //}
}
