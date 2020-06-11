using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Room currentRoom;

    public float moveSpeedWhenRoomChange;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPos();
    }

    void UpdateCameraPos()
    {
        if(currentRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPos();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeedWhenRoomChange * Time.deltaTime);
    }

    Vector3 GetCameraTargetPos()
    {
        if(currentRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currentRoom.GetRoomCenter();

        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPos()) == false;
    }
}
