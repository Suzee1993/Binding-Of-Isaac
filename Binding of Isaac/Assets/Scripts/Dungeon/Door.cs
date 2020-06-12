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
    private bool enemyRoom;
    private Spawner spawner;
    private BoxCollider2D doorCollider;

    private void Start()
    {
        room = FindObjectOfType<Room>();
        doorCollider = GetComponent<BoxCollider2D>();


        if (room.GetComponent<Spawner>() == null)
        {
            enemyRoom = false;
        }
        else
        {
            enemyRoom = true;
            spawner = room.GetComponent<Spawner>();
        }

        if (!enemyRoom)
        {
            doorCollider.isTrigger = true;
        }

    }

    private void Update()
    {
        if (enemyRoom && room.playerInRoom)
        {
            if(spawner.enemyCounter <= 0)
            {
                doorCollider.isTrigger = true;
            }
            else
            {
                doorCollider.isTrigger = false;
            }
        }
        if(enemyRoom && !room.playerInRoom)
        {
            doorCollider.isTrigger = true;
        }
    }
}
