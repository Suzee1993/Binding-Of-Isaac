using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<GameObject> InventorySlots = new List<GameObject>();

    public GameObject inventoryPanel;
    public bool inventoryActive = false;

    public TMP_Text itemTitle;
    public TMP_Text itemDescription;


    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void AddToInventory(Sprite sprite, string title, string description)
    {
        foreach (GameObject s in InventorySlots)
        {
            var slot = s.GetComponent<InventorySlot>();
            if (!slot.taken)
            {
                slot.sprite.GetComponent<Image>().sprite = sprite;

                itemTitle.text = title;
                itemDescription.text = description;


                slot.taken = true;
            }
            if (slot.taken)
            {
                InventorySlots.Remove(s);
            }
        }
    }

    public void RevomeFromInventory()
    {

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
