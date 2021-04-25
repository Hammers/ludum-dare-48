using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    private Tween _tween;
    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
       OnExitInteractionZone();
    }

    private void OnEnable()
    {
        _character.EnteredInteractionZone += OnEnterInteractionZone;
        _character.ExitedInteractionZone += OnExitInteractionZone;
    }

    private void OnDisable()
    {
        _character.EnteredInteractionZone -= OnEnterInteractionZone;
        _character.ExitedInteractionZone -= OnExitInteractionZone;
    }

    private void OnEnterInteractionZone()
    {
        _tween?.Kill();
        _tween = _rectTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.InOutQuad);
    }

    private void OnExitInteractionZone()
    {
        _tween?.Kill();
        _tween = _rectTransform.DOAnchorPosX(_rectTransform.sizeDelta.x, 0.2f).SetEase(Ease.InOutQuad);
    }
    
}
