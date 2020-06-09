using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{

    [Header("Inventory Slot Specific")]
    public bool taken = false;
    public Image spriteImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public float healthStat;
    public float speedStat;
    public float damageStat;
    public float attackSpeedStat;


    public Item.ItemTypes curItemType = Item.ItemTypes.Null;

    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        spriteImage.enabled = false;
    }

    public void ExecuteItem()//PlayerController player)
    {
        if (curItemType == Item.ItemTypes.Health)
        {
            GameController.Heal(healthStat);
            inventory.RevomeFromInventory(this.gameObject);
        }
        else if (curItemType == Item.ItemTypes.Speed)
        {
            GameController.SpeedUp(speedStat, damageStat, curItemType);
            inventory.RevomeFromInventory(this.gameObject);
        }
        else if (curItemType == Item.ItemTypes.AttackSpeed)
        {
            GameController.AttackSpeedUp(attackSpeedStat, speedStat, curItemType);
            inventory.RevomeFromInventory(this.gameObject);
        }
        else if (curItemType == Item.ItemTypes.Damage)
        {
            GameController.DamageUp(damageStat, curItemType);
            inventory.RevomeFromInventory(this.gameObject);
        }
    }

}
