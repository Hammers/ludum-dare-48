using System;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public event Action<int,int> CoinsCollected;
    
    [SerializeField] private int coins;

    public void AddCoins(int amount)
    {
        var prev = coins;
        coins += amount;
        CoinsCollected?.Invoke(prev,coins);
    }
}
