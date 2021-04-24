using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terminal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inActiveSprite;
    [SerializeField] private float interactionTime;
    [SerializeField] private Transform interactionPoint;

    [SerializeField] private InteractionBar _interactionBar;
    
    private Character _characterInRange;
    private bool _used;

    public void Start()
    {
        _interactionBar.gameObject.SetActive(false);
    }
    
    public void Interact()
    {
        StartCoroutine(InteractCo());
    }

    public IEnumerator InteractCo()
    {
        _spriteRenderer.color = Color.white;
        _interactionBar.gameObject.SetActive(true);
        _interactionBar._fillImage.fillAmount = 0;
        _interactionBar._fillImage.DOFillAmount(1f,interactionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(interactionTime);
        _interactionBar.gameObject.SetActive(false);
        _used = true;
        _spriteRenderer.sprite = _inActiveSprite;
        _characterInRange.RegainControl();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_used)
        {
            return;
        }

        var character = other.GetComponent<Character>();
        if(character != null)
        {
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
        if(character != null)
        {
            _characterInRange = null;
            _spriteRenderer.color = Color.white;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_used)
        {
            return;
        }

        if (_characterInRange != null)
        {
            _characterInRange.ForceToPosition(interactionPoint.position,Vector2.Angle(interactionPoint.position,transform.position) + 90,Interact);
        }
    }
}