using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

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
            if(roomLocation == dungeonRooms[dungeonRooms.Count - 1] && !(roomLocation == Vector2Int.zero))
            {
                RoomController.Instance.LoadRoom("End", roomLocation.x, roomLocation.y);
            }
            else
            {
                RoomController.Instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
            }
        }
    }
}
