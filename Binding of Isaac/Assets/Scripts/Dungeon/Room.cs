﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float width;
    public float height;
    public int xVal;
    public int yVal;

    public Room(int x, int y)
    {
        xVal = x;
        yVal = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door upDoor;
    public Door downDoor;

    public bool enemyRoom;
    public bool bossRoom;
    public bool itemRoom;

    public List<Door> doors = new List<Door>();

    public bool playerInRoom = false;
    public bool locked = false;
    public bool firstEntry = false;

    private bool updatedDoors = false;
    private Spawner spawner;
    private ItemSpawner itemSpawner;
    //public int GCKillCounter;
    public GameObject bossPanel;
    public bool bossPanelActivated = false;
    public GameObject roomHolder;

    public List<GameObject> enemiesInRoomList = new List<GameObject>();

    void Start()
    {
        if (bossRoom)
        {
            bossPanel.SetActive(false);
        }


        if(RoomController.Instance == null)
        {
            Debug.Log("You pressed play in de wrong scene.");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            doors.Add(d);

            switch (d.doorType)
            {
                case Door.DoorType.left:
                    leftDoor = d;
                    break;

                case Door.DoorType.right:
                    rightDoor = d;
                    break;

                case Door.DoorType.up:
                    upDoor = d;
                    break;

                case Door.DoorType.down:
                    downDoor = d;
                    break;
            }
        }

        RoomController.Instance.RegisterRoom(this);

        if (enemyRoom)
        {
            spawner = GetComponent<Spawner>();
            spawner.room = this;
        }
        if (!playerInRoom)
        {
            StartCoroutine(TurnOffRoomHolder());
        }
        if (itemRoom)
        {
            itemSpawner = GetComponent<ItemSpawner>();
            itemSpawner.room = this;
        }

    }

    private void Update()
    {
        if(name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }

        if (enemyRoom)
        {
            if (playerInRoom && spawner.enemyCounter > 0)
            {
                foreach (Door door in doors)
                {
                    door.CloseDoors();
                }
            }
            else
            {
                foreach (Door door in doors)
                {
                    if (!door.closedDoor)
                        door.OpenDoors();
                }
            }
        }
        else
        {
            foreach (Door door in doors)
            {
                if (!door.closedDoor)
                    door.OpenDoors();
            }
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.left:
                    if(GetLeft() == null)
                    {
                        DisableSpriteRenderers(door);
                    }
                    break;

                case Door.DoorType.right:
                    if (GetRight() == null)
                    {
                        DisableSpriteRenderers(door);
                    }
                    break;

                case Door.DoorType.up:
                    if (GetUp() == null)
                    {
                        DisableSpriteRenderers(door);
                    }
                    break;

                case Door.DoorType.down:
                    if (GetDown() == null)
                    {
                        DisableSpriteRenderers(door);
                    }
                    break;
            }
        }
    }

    public Room GetLeft()
    {
        if( RoomController.Instance.DoesRoomExist(xVal - 1, yVal))
        {
            return RoomController.Instance.FindRoom(xVal - 1, yVal);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetRight()
    {
        if (RoomController.Instance.DoesRoomExist(xVal + 1, yVal))
        {
            return RoomController.Instance.FindRoom(xVal + 1, yVal);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetUp()
    {
        if (RoomController.Instance.DoesRoomExist(xVal, yVal + 1))
        {
            return RoomController.Instance.FindRoom(xVal, yVal + 1);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetDown()
    {
        if (RoomController.Instance.DoesRoomExist(xVal, yVal - 1))
        {
            return RoomController.Instance.FindRoom(xVal, yVal - 1);
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0)); 
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(xVal * width, yVal * height);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RoomController.Instance.OnPlayerEnterRoom(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomHolder.SetActive(true);
            StartCoroutine(Wait());
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!firstEntry)
            {
                firstEntry = true;
            }

            playerInRoom = false;
            locked = false;

            roomHolder.SetActive(false);
        }
    }

    void DisableSpriteRenderers(Door door)
    {
        door.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        var doorchild = door.gameObject.transform.GetChild(0);
        doorchild.GetComponent<SpriteRenderer>().enabled = false;
        door.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        door.closedDoor = true;
    }

    IEnumerator TurnOffRoomHolder()
    {
        yield return new WaitForSecondsRealtime(2f);

        //if (enemyRoom)
        //{
        //    foreach (GameObject e in enemiesInRoomList)
        //    {
        //        //e.SetActive(false);
        //        e.gameObject.SetActive(false);
        //    }
        //}
        roomHolder.SetActive(false);
    }

    IEnumerator TurnOffEnemies()
    {
        yield return new WaitForSecondsRealtime(.1f);

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.6f);
        playerInRoom = true;

        if (bossRoom && spawner.bossRoom)
        {
            spawner.playerInBossRoom = true;
            if (!bossPanelActivated)
            {
                bossPanelActivated = true;
                bossPanel.SetActive(true);
            }

        }
    }
}
