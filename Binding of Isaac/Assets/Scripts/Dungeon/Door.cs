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

    public BoxCollider2D doorCollider;
    public SpriteRenderer sr;
    public Sprite bossRoomDoor;
    public Sprite bossRoomDoors;

    public LayerMask enemyMask;
    public Transform doorMaskTransform;
    public LayerMask doorMask;
    public GameObject doors;
    public float radius;
    public bool closedDoor = false;


    private SpriteRenderer babySR;

    private void Start()
    {
        babySR = doors.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, enemyMask))
        {
            CloseDoors();
        }
        else if(Physics2D.OverlapCircle(doorMaskTransform.position, radius, doorMask))
        {
            sr.sprite = bossRoomDoor;
            babySR.sprite = bossRoomDoors;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(doorMaskTransform.position, radius);
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
