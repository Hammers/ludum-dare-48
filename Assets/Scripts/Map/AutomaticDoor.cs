    using System;
    using UnityEngine;

    public class AutomaticDoor : MonoBehaviour, IDoor
    {
        [SerializeField] private Direction _direction;
        [SerializeField] private Animator _animator;

        public Direction Direction => _direction;
        public Transform Transform => transform;
        private bool _opened;

        public AudioClip audioClip;
        private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

        public void OpenDoor()
        {
            if (_opened)
            {
                return;
            }

            if (_animator)
            {
                _animator.SetBool("open", true);
            audioSource.PlayOneShot(audioClip,0.5f);
            }
            _opened = true;
            if (MapGenerator.Instance.GetRoomAtCellPos(
                MapGenerator.Instance.GetCellPosFromWorldPos(transform.position) + MapGenerator.DirToV2(_direction)) != null)
            {
                return;
            }
            
            MapGenerator.Instance.AddRandomRoom(MapGenerator.Instance.GetCellPosFromWorldPos(transform.position),_direction);
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
