using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance => _instance;
    private static MapGenerator _instance;
    
    
    [SerializeField] float _roomWorldSizeX = 1;
    [SerializeField] float _roomWorldSizeY = 1;
    [SerializeField] private Room _startRoom;
    [SerializeField] private List<Room> _roomPrefabs;
    [SerializeField] private int _startingWidth;
    [SerializeField] private int _startingHeight;
    private Dictionary<Vector2Int, Room> _rooms = new Dictionary<Vector2Int, Room>();

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("2 Map generators in scene!");
        }

        _instance = this;
    }
    
    void Start()
    {
        Room room = Instantiate(_startRoom,transform);
        AddRoomToMap(room,new Vector2Int(0,0));
    }

    public Room GetRoomAtCellPos(Vector2Int cellPos)
    {
        if (_rooms.ContainsKey(cellPos))
        {
            return _rooms[cellPos];
        }

        return null;
    }
    
    public Room GetRoomAtWorldPos(Vector3 worldPos)
    {
        return GetRoomAtCellPos(GetCellPosFromWorldPos(worldPos));
    }
    
    public Vector2Int GetCellPosFromWorldPos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / _roomWorldSizeX), Mathf.FloorToInt(worldPos.y / _roomWorldSizeY));
    }
    
    private bool CanAddRoomToMap(Room room, Vector2Int pos)
    {
        for (int x = pos.x; x < pos.x + room.Width; x++)
        {
            for (int y = pos.y; y < pos.y + room.Height; y++)
            {
                var cell = new Vector2Int(x, y);
                if (_rooms.ContainsKey(cell))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void AddRoomToMap(Room room, Vector2Int pos)
    {
        for (int x = pos.x; x < pos.x + room.Width; x++)
        {
            for (int y = pos.y; y < pos.y + room.Height; y++)
            {
                var cell = new Vector2Int(x, y);
                _rooms.Add(cell, room);
            }
        }
    }
    
    public void AddRandomRoom(Vector2Int currentPos, Vector2Int direction)
    {
        var newPos = currentPos + direction;
        List<Room> potentialRooms = new List<Room>(_roomPrefabs);
        Room selectedRoom = potentialRooms[Random.Range(0, potentialRooms.Count)];
        potentialRooms.Remove(selectedRoom);
        while (!CanAddRoomToMap(selectedRoom, newPos) && potentialRooms.Count > 0)
        {
            selectedRoom = potentialRooms[Random.Range(0, potentialRooms.Count)];
            potentialRooms.Remove(selectedRoom);
        }
        var spawnedRoom = Instantiate(selectedRoom, transform);
        spawnedRoom.transform.position = new Vector3(newPos.x * _roomWorldSizeX, newPos.y * _roomWorldSizeY);
        AddRoomToMap(spawnedRoom,newPos);
    }
}
