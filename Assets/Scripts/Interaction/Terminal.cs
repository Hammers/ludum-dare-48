using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Terminal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inActiveSprite;
    [SerializeField] private float interactionTime;
    [SerializeField] private Transform interactionPoint;

    [SerializeField] private InteractionBar _interactionBar;
    [SerializeField] private int _coins;

    public AudioClip audioClip;
    private AudioSource audioSource;

    public int Coins
    {
        get => _coins;
        set => _coins = value;
    }

    private Character _characterInRange;
    private bool _used;

    public void Start()
    {
        _interactionBar.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
        StartCoroutine(InteractCo());
    }

    public IEnumerator InteractCo()
    {
        _spriteRenderer.color = Color.white;
        _interactionBar.gameObject.SetActive(true);
        _interactionBar._fillImage.fillAmount = 0;
        _interactionBar._fillImage.DOFillAmount(1f, interactionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(interactionTime);
        _characterInRange.GetComponent<CharacterInventory>().AddCoins(Random.Range(_coins - 3, _coins + 3));
        _interactionBar.gameObject.SetActive(false);
        _spriteRenderer.sprite = _inActiveSprite;
        _characterInRange.RegainControl();
        _characterInRange.ExitInteractionZone();
        _characterInRange = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_used)
        {
            return;
        }

        var character = other.GetComponent<Character>();
        if (character != null)
        {
            character.EnterInteractionZone();
            _characterInRange = character;
            _spriteRenderer.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_used)
        {
            return;
        }
        
        
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            character.ExitInteractionZone();
            _characterInRange = null;
            _spriteRenderer.color = Color.white;
        }
    }

    public void Update()
    {
        if (_used)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_characterInRange != null)
            {
                _used = true;
                _characterInRange.ForceToPosition(interactionPoint.position,
                    Vector2.Angle(interactionPoint.position, transform.position) + 90, Interact);
            }
        }
    }
    
}