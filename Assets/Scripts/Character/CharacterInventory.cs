using System;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public event Action<int,int> CoinsCollected;
    public event Action CoinsReset;
    
    [SerializeField] private int coins;

    public void OnEnable()
    {
        GameManager.Instance.NewSessionStarted += OnSessionRestarted;
    }

    public void OnDisable()
    {
        GameManager.Instance.NewSessionStarted -= OnSessionRestarted;
    }

    private void OnSessionRestarted()
    {
        coins = 0;
        CoinsReset?.Invoke();
    }

    public void AddCoins(int amount)
    {
        var prev = coins;
        coins += amount;
        CoinsCollected?.Invoke(prev,coins);
    }
}
