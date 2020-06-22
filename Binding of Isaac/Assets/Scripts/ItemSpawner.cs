using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Items
    {
        public string itemName;
        public float weight;
    }

    public List<Items> itemNames = new List<Items>();

    public Room room;

    private float totalWeight;
    private bool hasSpawned = false;
    private string itemName;

    private void Awake()
    {
        totalWeight = 0;

        foreach (var items in itemNames)
        {
            totalWeight += items.weight;
        }
    }

    private void Start()
    {
        itemName = SelectItem();

    }

    private void Update()
    {
        if(room.playerInRoom && !hasSpawned)
        {
            hasSpawned = true;
            StartCoroutine(Wait(itemName));
        }
    }

    IEnumerator Wait(string itemName)
    {
        yield return new WaitForSeconds(.1f);
        SpawnItem(itemName);
    }

    void SpawnItem(string itemName)
    {
        GameObject item = PoolManager.Instance.SpawnFromPool(itemName) as GameObject;

        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
    }

    private string SelectItem()
    {
        float pick = Random.value * totalWeight;
        int index = 0;
        float cumulativeWeight = itemNames[0].weight;

        while (pick > cumulativeWeight)
        {
            index++;
            cumulativeWeight += itemNames[index].weight;
        }

        string selectedItemName = itemNames[index].itemName;

        return selectedItemName;
    }
}
