using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BankPrompt : MonoBehaviour
{
    public float displayTime;
    
    private RectTransform _rectTransform;
    private CharacterInventory characterInventory; 
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(-_rectTransform.sizeDelta.x,_rectTransform.anchoredPosition.y);
        characterInventory = FindObjectOfType<CharacterInventory>();
        characterInventory.CoinsCollected += OnCoinsCollected;
    }

    private void OnCoinsCollected(int arg1, int arg2)
    {
        characterInventory.CoinsCollected -= OnCoinsCollected;
        StartCoroutine(OnCoinsCollectedCo());
    }

    private IEnumerator OnCoinsCollectedCo()
    {
        _rectTransform.DOAnchorPosX(_rectTransform.sizeDelta.x, 0.2f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(displayTime);
        _rectTransform.DOAnchorPosX(-_rectTransform.sizeDelta.x, 0.2f).SetEase(Ease.InOutQuad);
        
    }
}