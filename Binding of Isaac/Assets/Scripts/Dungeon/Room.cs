using System.Collections;
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

    public List<Door> doors = new List<Door>();

    public bool playerInRoom = false;
    public bool locked = false;
    public bool firstEntry = false;

    private bool updatedDoors = false;
    private Spawner spawner;
    public int GCKillCounter;

    void Start()
    {
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
            GCKillCounter = GameController.instance.enemyKillCounter;
            if (playerInRoom && (GameController.instance.enemyKillCounter != GCKillCounter + spawner.enemyCounter))
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
                    door.OpenDoors();
                }
            }
        }
        else
        {
            foreach (Door door in doors)
            {
                door.OpenDoors();
            }
        }


        //if(enemyRoom && playerInRoom && !locked && firstEntry)
        //{
        //    locked = true;

        //    foreach (Door door in doors)
        //    {
        //        GMKillCounter = GameController.instance.enemyKillCounter;
        //        door.CloseDoors();
        //    }
        //}

        //if(enemyRoom && (GMKillCounter + spawner.enemyCounter) == GameController.instance.enemyKillCounter)
        //{
        //    foreach (Door door in doors)
        //    {
        //        spawner.enemiesInRoom = false;
        //        door.OpenDoors();
        //    }
        //}

        //if (!firstEntry)
        //{
        //    foreach (Door door in doors)
        //    {
        //        door.OpenDoors();
        //    }
        //}
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
            //playerInRoom = true;


        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Wait());
            //playerInRoom = true;
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
        }
    }

    void DisableSpriteRenderers(Door door)
    {
        door.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        var doorchild = door.gameObject.transform.GetChild(0);
        doorchild.GetComponent<SpriteRenderer>().enabled = false;
        door.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.6f);
        playerInRoom = true;
    }
}
