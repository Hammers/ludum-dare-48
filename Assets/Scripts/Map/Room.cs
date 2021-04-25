using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private List<GameObject> _doors; 

    public int Width => width;
    public int Height => height;
    public List<IDoor> Doors => _doors.Select(x => x.GetComponent<IDoor>()).ToList();

    public bool HasDoor(Vector2Int localCellPos, Direction dir)
    {
            return Doors.Any(x => MapGenerator.Instance.GetCellPosFromWorldPos(x.Transform.localPosition) == localCellPos && x.Direction == dir);
    }
}