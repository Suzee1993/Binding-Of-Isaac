using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    protected override void ExecuteItem(PlayerController player)
    {
        if(player.health < player.maxHealth)
        {
            player.health += healthStat;
            if (player.health > player.maxHealth)
            {
                player.health = player.maxHealth;

                PlayerController.collectedAmount += 1;
                //Turn Off Object
                base.ExecuteItem(player);
            } 
            else
            {
                //TODO: ADD TO INVENTORY
            }
        }
    }
}
