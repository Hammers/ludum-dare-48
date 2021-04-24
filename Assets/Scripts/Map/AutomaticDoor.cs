    using System;
    using UnityEngine;

    public class AutomaticDoor : MonoBehaviour
    {
        [SerializeField] private Vector2Int direction;

        private bool _spawned;

        private void Start()
        {
            if (MapGenerator.Instance.GetRoomAtCellPos(
                MapGenerator.Instance.GetCellPosFromWorldPos(transform.position) + direction) != null)
            {
                _spawned = true;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_spawned)
            {
                return;
            }
            if (MapGenerator.Instance.GetRoomAtCellPos(
                MapGenerator.Instance.GetCellPosFromWorldPos(transform.position) + direction) != null)
            {
                _spawned = true;
                return;
            }
            var player = other.GetComponent<CharacterMovement>();
            if (player)
            {
                Debug.Log("Player entered door");
                MapGenerator.Instance.AddRandomRoom(MapGenerator.Instance.GetCellPosFromWorldPos(transform.position),direction);
                _spawned = true;
            }
        }
    }
