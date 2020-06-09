using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.WSA.Input;

public class Inventory : MonoBehaviour
{
    public List<GameObject> InventorySlots = new List<GameObject>();
    private List<GameObject> InvSlots;

    public GameObject inventoryPanel;
    public bool inventoryActive = false;
    //Queue<GameObject> slots = new Queue<GameObject>();

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void AddToInventory(Sprite sprite, string title, string description, Item.ItemTypes iType, float damageStat, float speedStat, float attackSpeedStat, float healthStat)
    {
        foreach (GameObject s in InventorySlots)
        {
            var slot = s.GetComponent<InventorySlot>();
            if (!slot.taken)
            {
                Image image = slot.spriteImage.GetComponent<Image>();
                image.enabled = true;
                image.sprite = sprite;
                slot.titleText.text = title;
                slot.descriptionText.text = description;
                slot.curItemType = iType;
                slot.healthStat = healthStat;
                slot.damageStat = damageStat;
                slot.speedStat = speedStat;
                slot.attackSpeedStat = attackSpeedStat;

                slot.taken = true;
            }
            if (slot.taken)
            {
                InventorySlots.Remove(s);
            }
        }
    }

    public void RevomeFromInventory(GameObject inventorySlot)
    {
        Debug.Log(gameObject.name + "Clicked");
        var invs = inventorySlot.GetComponent<InventorySlot>();
        invs.spriteImage.GetComponent<Image>().enabled = false;
        invs.titleText.text = "Empty Slot";
        invs.descriptionText.text = "";
        invs.taken = false;
        
        InventorySlots.Add(inventorySlot);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!inventoryActive)
            {
                StartCoroutine(Wait(true));
                inventoryPanel.SetActive(true);
                Debug.Log("Inventory Active");
            }
            if (inventoryActive)
            {

                StartCoroutine(Wait(false));
                inventoryPanel.SetActive(false);
                Debug.Log("Inventory InActive");
            }

        }
    }

    IEnumerator Wait(bool value)
    {
        yield return new WaitForSeconds(.2f);
        inventoryActive = value;
    }
}
