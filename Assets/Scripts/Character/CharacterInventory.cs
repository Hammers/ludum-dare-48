using System;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public event Action<int,int> CoinsCollected;
    public event Action CoinsReset;
    
    [SerializeField] public int coins;

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
        ClearCoins();
    }

    public void ClearCoins(){
        coins = 0;
        CoinsReset?.Invoke();
    }

    public void AddCoins(int amount)
    {
        var prev = coins;
        coins += amount;
        CoinsCollected?.Invoke(prev,coins);
    }

    public int GetCoins(){
        return coins;
    }
}
