using System;
using DG.Tweening;
using UnityEngine;


public class Character : MonoBehaviour
{
    public event Action EnteredInteractionZone;
    public event Action ExitedInteractionZone;
    
    private CharacterMovement _characterMovement;
    private CharacterRotation _characterRotation;
    private Rigidbody2D _rb;
    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterRotation = GetComponent<CharacterRotation>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void EnterInteractionZone()
    {
        EnteredInteractionZone?.Invoke();
    }
    
    public void ExitInteractionZone()
    {
        ExitedInteractionZone?.Invoke();
    }
    
    public void OverrideMovement()
    {
        _characterMovement.enabled = false;
        _characterRotation.enabled = false;
        _rb.isKinematic = true;
    }

    public void ForceToPosition(Vector2 pos, float angle, Action onComplete,float duration = 0.5f)
    {
        OverrideMovement();
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _rb.DORotate(angle, duration);
        _rb.DOMove(pos,duration).onComplete = () => onComplete();
    }

    public void RegainControl()
    {
        _characterMovement.enabled = true;
        _characterRotation.enabled = true;
        _rb.isKinematic = false;
    }
}