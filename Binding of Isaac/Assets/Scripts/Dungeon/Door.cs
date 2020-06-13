using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left,
        right,
        up,
        down
    };

    public DoorType doorType;

    private Room room;
    private Spawner spawner;
    private BoxCollider2D doorCollider;

    private void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();

        doorCollider.isTrigger = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            doorCollider.isTrigger = false;
        }
    }

    public void CloseDoors()
    {
        doorCollider.isTrigger = false;
    }

    public void OpenDoors()
    {
        doorCollider.isTrigger = true;
    }
}
