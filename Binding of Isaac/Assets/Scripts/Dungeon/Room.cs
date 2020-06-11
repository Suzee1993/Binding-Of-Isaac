using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float width;
    public float height;
    public int x;
    public int y;

    public Door leftDoor;
    public Door rightDoor;
    public Door upDoor;
    public Door downDoor;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
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
                        door.gameObject.SetActive(false);
                    }
                    break;

                case Door.DoorType.right:
                    if (GetRight() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;

                case Door.DoorType.up:
                    if (GetUp() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;

                case Door.DoorType.down:
                    if (GetDown() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    public Room GetLeft()
    {
        if( RoomController.Instance.DoesRoomExist(x - 1, y))
        {
            return RoomController.Instance.FindRoom(x - 1, y);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetRight()
    {
        if (RoomController.Instance.DoesRoomExist(x + 1, y))
        {
            return RoomController.Instance.FindRoom(x + 1, y);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetUp()
    {
        if (RoomController.Instance.DoesRoomExist(x, y + 1))
        {
            return RoomController.Instance.FindRoom(x, y + 1);
        }
        else
        {
            return null;
        }
    }    
    
    public Room GetDown()
    {
        if (RoomController.Instance.DoesRoomExist(x, y - 1))
        {
            return RoomController.Instance.FindRoom(x, y - 1);
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
        return new Vector3(x * width, y * height);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RoomController.Instance.OnPlayerEnterRoom(this);
        }
    }
}
