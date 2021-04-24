    using System;
    using UnityEngine;

    public class AutomaticDoor : MonoBehaviour
    {
        [SerializeField] private Direction _direction;
        [SerializeField] private Animator _animator;
        
        private bool _opened;

        public void OpenDoor()
        {
            if (_opened)
            {
                return;
            }

            if (_animator)
            {
                _animator.SetBool("open", true);
            }
            Vector2Int dir = Vector2Int.zero;
            switch (_direction)
            {
                case Direction.Up:
                    dir = Vector2Int.up;
                    break;
                case Direction.Down:
                    dir = Vector2Int.down;
                    break;
                case Direction.Left:
                    dir = Vector2Int.left;
                    break;
                case Direction.Right:
                    dir = Vector2Int.right;
                    break;
            }
            _opened = true;
            if (MapGenerator.Instance.GetRoomAtCellPos(
                MapGenerator.Instance.GetCellPosFromWorldPos(transform.position) + dir) != null)
            {
                return;
            }
            
            MapGenerator.Instance.AddRandomRoom(MapGenerator.Instance.GetCellPosFromWorldPos(transform.position),dir);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<CharacterMovement>();
            if (player)
            {
                Debug.Log("Player entered door");
                OpenDoor();
            }
            
        }
    }
