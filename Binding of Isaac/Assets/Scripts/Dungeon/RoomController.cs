using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;
    public int xVal;
    public int yVal;
}

public class RoomController : MonoBehaviour
{
    public static RoomController Instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRoom;

    Room currentRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if(loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRoom)
            {
                foreach ( Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRoom = true;
            }

            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    public void LoadRoom(string sceneName, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();

        newRoomData.name = sceneName;
        newRoomData.xVal = x;
        newRoomData.yVal = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    public void RegisterRoom(Room room)
    {
        if(!DoesRoomExist(currentLoadRoomData.xVal, currentLoadRoomData.yVal))
        {
            room.transform.position = new Vector3(currentLoadRoomData.xVal * room.width, currentLoadRoomData.yVal * room.height, 0);

            room.xVal = currentLoadRoomData.xVal;
            room.yVal = currentLoadRoomData.yVal;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.xVal + "," + room.yVal;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.Instance.currentRoom = room;
            }

            loadedRooms.Add(room);
            //room.RemoveUnconnectedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.xVal == x && item.yVal == y) != null;
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.Instance.currentRoom = room;
        currentRoom = room;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.xVal == x && item.yVal == y);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        //Async operation
        //LoadSceneMode.Additive = So the scenes will overlap and the rooms will be put in the same scene so I can use/access them
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;

        yield return new WaitForSeconds(0.5f);

        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.xVal, bossRoom.yVal);

            Destroy(bossRoom.gameObject);

            var roomToRemove = loadedRooms.Single(r => r.xVal == tempRoom.xVal && r.yVal == tempRoom.yVal);

            if (!roomToRemove.name.Contains("Empty"))
            {
                roomToRemove.GetComponent<Spawner>().bossRoom = true;
            }

            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.xVal, tempRoom.yVal);
        }
    }

}
