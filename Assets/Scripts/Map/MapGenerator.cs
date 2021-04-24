using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] float _roomWorldSizeX = 1;
    [SerializeField] float _roomWorldSizeY = 1;
    [SerializeField] private Room _startRoom;
    [SerializeField] private List<Room> _roomPrefabs;
    [SerializeField] private int _startingWidth;
    [SerializeField] private int _startingHeight;
    private Dictionary<Vector2Int, Room> _rooms = new Dictionary<Vector2Int, Room>();

    void Start()
    {
        GenerateLayout();
    }
    
    private void GenerateLayout()
    {
        for (int x = 0; x < _startingWidth; x++)
        {
            for (int y = 0; y < _startingHeight; y++)
            {
                Room room;
                if (x == 0 && y == 0)
                {
                    room = Instantiate(_startRoom,transform);
                }
                else
                {
                    room = Instantiate(_roomPrefabs[Random.Range(0, _roomPrefabs.Count - 1)],transform);
                }

                room.transform.position = new Vector3(x * _roomWorldSizeX, y * _roomWorldSizeY);
                _rooms.Add(new Vector2Int(x, y), room);
            }
        }
    }
}
