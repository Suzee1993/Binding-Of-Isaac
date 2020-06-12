using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public float columns, rows;
        public float vertOffset, horOffset;
    }

    public Grid grid;
    public GameObject tile;

    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();

        grid.columns = room.width + 2;
        grid.rows = room.height + 2;

        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.vertOffset += room.GetComponent<Transform>().localPosition.y;
        grid.horOffset += room.GetComponent<Transform>().localPosition.x;

        for (int y = 0; y < grid.rows; y++)
        {
            for (int x = 0; x < grid.columns; x++)
            {
                GameObject go = Instantiate(tile, transform);
                go.transform.position = new Vector2(x - (grid.columns - grid.horOffset), y - (grid.rows - grid.vertOffset));
                go.name = "x: " + x + ", y: " + y;
                availablePoints.Add(go.transform.position);
            }
        }

    }
}
