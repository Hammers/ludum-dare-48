using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance => _instance;
    private static MapGenerator _instance;
    
    
    [SerializeField] float _roomWorldSizeX = 1;
    [SerializeField] float _roomWorldSizeY = 1;
    [SerializeField] private Room _startRoom;
    [SerializeField] private List<Room> _roomPrefabs;
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
    
    private bool CanAddRoomToMap(Room room, Vector2Int pos, Direction dir, out Vector2Int chosenSpawnCell)
    {
        List<IDoor> allDoors = room.Doors;
        List<IDoor> oppositeDoors = allDoors.FindAll(x => x.Direction == OppositeDirection(dir));
        if (oppositeDoors.Count == 0)
        {
            chosenSpawnCell = pos;
            return false;
        }

        oppositeDoors.Shuffle();
        foreach (var door in room.Doors)
        {
            bool collides = false;
            Vector2Int doorCellPos = GetCellPosFromWorldPos(door.Transform.localPosition);
            Vector2Int bottomPos = pos - doorCellPos;
            for (int x = bottomPos.x; x < bottomPos.x + room.Width; x++)
            {
                for (int y = bottomPos.y; y < bottomPos.y + room.Height; y++)
                {
                    var cell = new Vector2Int(x, y);
                    if (_rooms.ContainsKey(cell))
                    {
                        collides = true;
                    }
                }
            }

            if (!collides)
            {
                chosenSpawnCell = bottomPos;
                return true;
            }
        }
        
        chosenSpawnCell = pos;
        return false;
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
    
    public void AddRandomRoom(Vector2Int currentPos, Direction direction)
    {
        var newPos = currentPos + DirToV2(direction);
        List<Room> potentialRooms = new List<Room>(_roomPrefabs);
        Room selectedRoom = potentialRooms[Random.Range(0, potentialRooms.Count)];
        potentialRooms.Remove(selectedRoom);
        Vector2Int spawnPos;
        while (!CanAddRoomToMap(selectedRoom, newPos, direction, out spawnPos) && potentialRooms.Count > 0)
        {
            selectedRoom = potentialRooms[Random.Range(0, potentialRooms.Count)];
            potentialRooms.Remove(selectedRoom);
        }
        var spawnedRoom = Instantiate(selectedRoom, transform);
        spawnedRoom.transform.position = new Vector3(spawnPos.x * _roomWorldSizeX, spawnPos.y * _roomWorldSizeY);
        AddRoomToMap(spawnedRoom,spawnPos);
    }

    public static Vector2Int DirToV2(Direction dir)
    {
        switch(dir)
        {
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Right:
                return Vector2Int.right;
        }
        return Vector2Int.zero;
    }

    public static Direction OppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }

        return Direction.Up;
    }
}
