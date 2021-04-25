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

    public AudioClip audioClip;
    private AudioSource audioSource;

    private Character _characterInRange;

    public Character CharacterInRange => _characterInRange;
    private bool _used;

    public void Start()
    {
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

    }
    
    public IEnumerator InteractCo()
    {
        _spriteRenderer.color = Color.white;
        _interactionBar.gameObject.SetActive(true);
        _interactionBar._fillImage.fillAmount = 0;
        _interactionBar._fillImage.DOFillAmount(1f, interactionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(interactionTime);
        UseTerminal();
        _interactionBar.gameObject.SetActive(false);
        _spriteRenderer.sprite = _inActiveSprite;
        _characterInRange.RegainControl();
        _characterInRange.ExitInteractionZone();
        _characterInRange = null;
        GameManager.Instance.AddUsedTerminal(this);
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