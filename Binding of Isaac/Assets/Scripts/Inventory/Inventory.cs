using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.WSA.Input;

public class Inventory : MonoBehaviour
{
    [Header("List")]
    public List<GameObject> InventorySlots = new List<GameObject>();
    private List<GameObject> InvSlots;
    [Header("Array")]
    public bool[] isFull;
    public GameObject[] inventorySlots;

    [Header("Else")]
    public GameObject inventoryPanel;
    public bool inventoryActive = false;
    //Queue<GameObject> slots = new Queue<GameObject>();

    public bool inventoryFull = false;

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void AddToInventory(Sprite sprite, string title, string description, Item.ItemTypes iType, float damageStat, float speedStat, float attackSpeedStat, float healthStat)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (isFull[i] == false)
            {
                var slot = inventorySlots[i].GetComponent<InventorySlot>();

                Image image = slot.spriteImage.GetComponent<Image>();
                //image.enabled = true;

                //Color temp = Color.white;
                //temp.a = 255f;
                image.color = Color.white;

                image.sprite = sprite;
                slot.titleText.text = title;
                slot.descriptionText.text = description;
                slot.curItemType = iType;
                slot.healthStat = healthStat;
                slot.damageStat = damageStat;
                slot.speedStat = speedStat;
                slot.attackSpeedStat = attackSpeedStat;

                isFull[i] = true;
                break;
            }

            if(isFull[6] == true)
            {
                inventoryFull = true;
            }
        }
    }

    public void RevomeFromInventory(GameObject inventorySlot, int i)
    {
        Debug.Log(gameObject.name + "Clicked");
        var invs = inventorySlot.GetComponent<InventorySlot>();
        invs.spriteImage.GetComponent<Image>().enabled = false;
        invs.titleText.text = "Empty Slot";
        invs.descriptionText.text = "";
        isFull[i] = false;

        invs.taken = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
