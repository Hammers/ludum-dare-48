using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Terminal : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Sprite _activeSprite;
    [SerializeField] protected Sprite _inActiveSprite;
    [SerializeField] private float interactionTime;
    [SerializeField] private Transform interactionPoint;

    [SerializeField] private InteractionBar _interactionBar;

    public AudioClip audioClip;
    private AudioSource audioSource;
    private Color originalColour;

    private Character _characterInRange;

    public Character CharacterInRange => _characterInRange;
    protected bool _used;
    private bool _using;
    public void Start()
    {
        originalColour = _spriteRenderer.color;
        _interactionBar.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Reset()
    {
        _spriteRenderer.sprite = _activeSprite;
        _used = false;
        _spriteRenderer.color = Color.white;
    }
    
    public void Interact()
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
        StartCoroutine(InteractCo());
    }

    protected virtual void UseTerminal()
    {
        EndActivation();
    }
    
    public IEnumerator InteractCo()
    {
        _spriteRenderer.color = originalColour;
        _interactionBar.gameObject.SetActive(true);
        _interactionBar._fillImage.fillAmount = 0;
        _interactionBar._fillImage.DOFillAmount(1f, interactionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(interactionTime);
        UseTerminal();
    }

    public void EndActivation()
    {
        _using = false;
        _interactionBar.gameObject.SetActive(false);
        _characterInRange.RegainControl();
        _characterInRange.ExitInteractionZone();
        _characterInRange = null;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_used || _using)
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
        if (_used || _using)
        {
            return;
        }
        
        
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            character.ExitInteractionZone();
            _characterInRange = null;
            _spriteRenderer.color = originalColour;
        }
    }

    public void Update()
    {
        if (_used || _using)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_characterInRange != null)
            {
                _using = true;
                _characterInRange.ForceToPosition(interactionPoint.position,
                    Vector2.Angle(interactionPoint.position, transform.position) + 90, Interact);
            }
        }
    }
    
}