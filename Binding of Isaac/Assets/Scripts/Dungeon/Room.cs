using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float width;
    public float height;
    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.Instance == null)
        {
            Debug.Log("You pressed play in de wrong scene.");
            return;
        }

        RoomController.Instance.RegisterRoom(this);
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
}
