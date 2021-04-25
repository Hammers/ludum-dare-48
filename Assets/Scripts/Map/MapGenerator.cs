using System;
using System.Linq;
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

    [SerializeField] private Terminal _terminalPrefab;
    [SerializeField] float _roomWorldSizeX = 1;
    [SerializeField] float _roomWorldSizeY = 1;
    [SerializeField] private float _distanceTreasureMultiplier = 1.5f;
    [SerializeField] private int _maxRoomsWithoutTreasure = 5;
    [SerializeField] private Room _startRoom;
    [SerializeField] private List<Room> _roomPrefabs;
    [SerializeField] private List<Room> _safetyRoomPrefabs;
    private Dictionary<Vector2Int, Room> _rooms = new Dictionary<Vector2Int, Room>();
    private int _roomsWithoutTreasure = 0;
    
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
        //Debug.Log($"--Checking {room.name}- spawn: {pos}, dir: {dir.ToString()} ");
        List<IDoor> allDoors = room.Doors;
        List<IDoor> oppositeDoors = allDoors.FindAll(x => x.Direction == OppositeDirection(dir));
        Debug.Log($"--Checking number of doors- allDoors: {allDoors.Count}, oppositeDoor:{oppositeDoors.Count}");
        if (oppositeDoors.Count == 0)
        {
            //Debug.Log($"No Opposite doors (direction {OppositeDirection(dir)}");
            chosenSpawnCell = pos;
            return false;
        }

        //oppositeDoors.Shuffle();
        Debug.Log($"-- Going through opposite doors");
        foreach (var door in oppositeDoors)
        {   

            bool collides = false;
            Vector2Int doorCellPos = GetCellPosFromWorldPos(door.Transform.localPosition);
            //Debug.Log($"----Checking door at local cell{doorCellPos} ");
            Vector2Int bottomPos = pos - doorCellPos;
            for (int x = bottomPos.x; x < bottomPos.x + room.Width; x++)
            {
                for (int y = bottomPos.y; y < bottomPos.y + room.Height; y++)
                {
                    var cell = new Vector2Int(x, y);
                    if (_rooms.ContainsKey(cell))
                    {
                        //Debug.Log($"----Collided at {cell}");
                        collides = true;
                    }
                }
            }

            if (!collides)
            {
                //Debug.Log($"----No Collisions!");

                bool doorMismatch = false;
                for (int x = bottomPos.x; x < bottomPos.x + room.Width;x++)
                {
                    Vector2Int checkingCell = new Vector2Int(x, bottomPos.y);
                    Vector2Int oppositeCell = new Vector2Int(x, bottomPos.y - 1);
                    Room r = GetRoomAtCellPos(oppositeCell);
                    if (r != null && r.HasDoor(oppositeCell- GetCellPosFromWorldPos(r.transform.position), Direction.Up) != room.HasDoor(checkingCell - bottomPos, Direction.Down))
                    {
                        //Debug.Log($"----No matching door - existing room:{oppositeCell- GetCellPosFromWorldPos(r.transform.position)}, new room: {checkingCell - bottomPos}");
                        doorMismatch = true;
                        break;
                    }
                    checkingCell = new Vector2Int(x, bottomPos.y + (room.Height -1));
                    oppositeCell = new Vector2Int(x, bottomPos.y + room.Height);
                    r = GetRoomAtCellPos(oppositeCell);
                    if (r != null && r.HasDoor(oppositeCell- GetCellPosFromWorldPos(r.transform.position), Direction.Down) != room.HasDoor(checkingCell - bottomPos, Direction.Up))
                    {
                        //Debug.Log($"----No matching door - existing room:{oppositeCell- GetCellPosFromWorldPos(r.transform.position)}, new room: {checkingCell - bottomPos}");

                        doorMismatch = true;
                        break;
                    }
                }
                for (int y = bottomPos.y; y < bottomPos.y + room.Height;y++)
                {
                    Vector2Int checkingCell = new Vector2Int(bottomPos.x, y);
                    Vector2Int oppositeCell = new Vector2Int(bottomPos.x - 1, y);
                    Room r = GetRoomAtCellPos(oppositeCell);
                    if (r != null && r.HasDoor(oppositeCell - GetCellPosFromWorldPos(r.transform.position), Direction.Right) != room.HasDoor(checkingCell - bottomPos, Direction.Left))
                    {
                        //Debug.Log($"----No matching door - existing room:{oppositeCell- GetCellPosFromWorldPos(r.transform.position)}, new room: {checkingCell - bottomPos}");
                        doorMismatch = true;
                        break;
                    }
                    checkingCell = new Vector2Int( bottomPos.x + (room.Width - 1),y);
                    oppositeCell = new Vector2Int( bottomPos.x + room.Width,y);
                    r = GetRoomAtCellPos(oppositeCell);
                    if (r != null && r.HasDoor(oppositeCell- GetCellPosFromWorldPos(r.transform.position), Direction.Left) != room.HasDoor(checkingCell - bottomPos, Direction.Right))
                    {
                        //Debug.Log($"----No matching door - existing room:{oppositeCell- GetCellPosFromWorldPos(r.transform.position)}, new room: {checkingCell - bottomPos}");
                        doorMismatch = true;
                        break;
                    }
                }

                if (!doorMismatch)
                {
                    //Debug.Log($"----No Doormismatch!");
                    chosenSpawnCell = bottomPos;
                    return true;
                }
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
                Debug.Log($"--Adding {room.name} at {cell}");

                _rooms.Add(cell, room);
            }
        }
    }
    
    public void AddRandomRoom(Vector2Int currentPos, Direction direction)
    {
        var newPos = currentPos + DirToV2(direction);
        Room currentRoom = GetRoomAtCellPos(currentPos);
        Debug.Log($"Attempting to spawn at cell pos {newPos}");
        List<Room> potentialRooms = new List<Room>(_roomPrefabs);
        Room selectedRoom = null;
        Vector2Int spawnPos;
        bool addedSafetyRooms = false;
        bool requireTreaureRoom = _roomsWithoutTreasure == _maxRoomsWithoutTreasure;
        if (requireTreaureRoom)
        {
            Debug.Log("Forcing treasure room");
        }
        while (true)
        {
            //Add the safety rooms to the ppol if we ran out of regular rooms
            if (potentialRooms.Count == 0)
            {
                if (!addedSafetyRooms)
                {
                    potentialRooms.AddRange(_safetyRoomPrefabs);
                    addedSafetyRooms = true;
                }
                else
                {
                    Debug.LogError("No rooms to spawn");
                    return;   
                }
            }
            // Select a new room
            selectedRoom = potentialRooms[Random.Range(0, potentialRooms.Count)];
            potentialRooms.Remove(selectedRoom);
            
            //IS this room the same as the last?
            if (!addedSafetyRooms && selectedRoom.name == currentRoom.name)
            {
                continue;
            }

            // Do we need a treasure room?
            if (!addedSafetyRooms && requireTreaureRoom && selectedRoom.Terminals.Count == 0)
            {
                continue;
            }
            
            // Does this room fit in the map?
            if (!CanAddRoomToMap(selectedRoom, newPos, direction, out spawnPos))
            {
                continue;
            }

            // This room is good! Stop searching
            break;
        }
        Debug.Log($"--Spawning {selectedRoom.name} at {spawnPos}");
        var spawnedRoom = Instantiate(selectedRoom, transform);
        spawnedRoom.name = selectedRoom.name;
        spawnedRoom.transform.position = new Vector3(spawnPos.x * _roomWorldSizeX, spawnPos.y * _roomWorldSizeY);
        List<Terminal> terminals = spawnedRoom.Terminals;
        if (addedSafetyRooms && requireTreaureRoom)
        {
            Terminal terminal = Instantiate(_terminalPrefab);
            terminal.transform.position =
                spawnedRoom.transform.position + new Vector3(_roomWorldSizeX / 2, _roomWorldSizeY / 2);
            terminals.Add(terminal);
        }
        foreach (var terminal in terminals.Where(x => x is CoinTerminal).Cast<CoinTerminal>())
        {
            int newcoins = Mathf.RoundToInt(terminal.Coins * ((Mathf.Abs(newPos.x) + Mathf.Abs(newPos.y)) * _distanceTreasureMultiplier));
            Debug.Log($"Setting terminal coins to {newcoins}");
            terminal.Coins = newcoins;
        }

        if (terminals.Count == 0)
        {
            _roomsWithoutTreasure++;
        }
        else
        {
            _roomsWithoutTreasure = 0;
        }
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
