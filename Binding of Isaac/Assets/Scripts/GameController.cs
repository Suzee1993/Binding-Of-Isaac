using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health = 6;
    private static float maxHealth;
    private static float speed = 2.2f;
    private static float fireDelay = 0.5f;
    private static float damage = 1f;

    public static float Health { get => health; set => health = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float Speed { get => speed; set => speed = value; }
    public static float FireDelay { get => fireDelay; set => fireDelay = value; }
    public static float Damage { get => damage; set => damage = value; }

    private static float playerDamage;
    private static float playerSpeed;
    private static float playerMoveSpeed;
    private static float playerAttackSpeed;

    public int enemyKillCounter = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        maxHealth = health;
    }

    public static void TakeDamage(float damage)
    {
        health -= damage;

        if(Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void Heal(float healAmount)
    {
        //TODO: Add to Inventory
        if (Health < MaxHealth)
        {
            health = Mathf.Min(maxHealth, health + healAmount);

            if (Health > MaxHealth)
            {
                Health = MaxHealth;

                PlayerController.collectedAmount += 1;
            }
            else
            {
                //TODO: ADD TO INVENTORY
            }
        }
    }

    public static void SpeedUp(float speedAmount, float damageAmount, Item.ItemTypes itemType) //, float time
    {
        playerMoveSpeed = Speed;
        playerDamage = Damage;

        Speed += speedAmount;
        Damage *= damageAmount;

        Debug.Log(Speed);
        Debug.Log(Damage);

        //StartCoroutine(Timer(playerMoveSpeed, playerDamage, time, itemType));

        PlayerController.collectedAmount += 1;
    }

    public static void AttackSpeedUp(float attackSpeedAmount, float speedAmount,  Item.ItemTypes itemtype) //float time,
    {
        playerAttackSpeed = FireDelay; //adjust down to fire faster
        playerMoveSpeed = Speed; //adjust down to move slower

        FireDelay *= attackSpeedAmount;
        Speed *= speedAmount;

        //StartCoroutine(Timer(attackSpeedAmount, speedAmount, time, itemtype));

        PlayerController.collectedAmount += 1;
    }

    public static void DamageUp(float damageAmount, Item.ItemTypes itemType) //float time,
    {
        playerDamage = Damage;
        Damage *= damageAmount;

        //StartCoroutine(Timer(playerDamage, 0.0f, time, itemType));

        PlayerController.collectedAmount += 1;
    }

    private static void KillPlayer()
    {

    }


    #region Timer
    //public static IEnumerator Timer(float prevPlayerStat, float prevPlayerStat2, float time, Item.ItemTypes itemType)
    //{
    //    Debug.Log("Timer Start");
    //    yield return new WaitForSeconds(time);

    //    if (itemType == Item.ItemTypes.Health)
    //    {

    //    }
    //    else if (itemType == Item.ItemTypes.Speed)
    //    {
    //        speed = prevPlayerStat;
    //        damage = prevPlayerStat2;
    //    }
    //    else if (itemType == Item.ItemTypes.AttackSpeed)
    //    {
    //        fireDelay = prevPlayerStat;
    //        speed = prevPlayerStat2;
    //    }
    //    else if (itemType == Item.ItemTypes.Damage)
    //    {
    //        damage = prevPlayerStat;
    //    }

    //    Debug.Log("Timer End");
    //}
    #endregion
}
