using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    [System.Serializable]
    public struct Selectable
    {
        public string roomName;
        public float weight;
    }

    public List<Selectable> roomNames = new List<Selectable>();

    private float totalWeight;

    private void Awake()
    {
        totalWeight = 0;

        foreach (var selectable in roomNames)
        {
            totalWeight += selectable.weight;
        }
    }

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.Instance.LoadRoom("Start", 0, 0);

        foreach (Vector2Int roomLocation in rooms)
        {
            #region Comment out
            //if(roomLocation == dungeonRooms[dungeonRooms.Count - 1] && !(roomLocation == Vector2Int.zero))
            //{
            //RoomController.Instance.LoadRoom("End", roomLocation.x, roomLocation.y);
            // }
            //else
            //{
            //}
            #endregion
            RoomController.Instance.LoadRoom(SelectRoom(), roomLocation.x, roomLocation.y); // "Empty"

        }
    }

    private string SelectRoom()
    {
        float pick = Random.value * totalWeight;
        int index = 0;
        float cumulativeWeight = roomNames[0].weight;

        while (pick > cumulativeWeight && index < dungeonGenerationData.iterationMin)
        {
            index++;
            cumulativeWeight += roomNames[index].weight;
        }

        string selectedRoomName = roomNames[index].roomName;

        return selectedRoomName;


        //string selectedRoomName;
        //int index;

        //index = Random.Range(0, roomNames.Count);
        //selectedRoomName = roomNames[index];

        //return selectedRoomName;
    }
}
