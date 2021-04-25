using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTerminal : Terminal
{
    [SerializeField] private int _coins;
    public int Coins
    {
        get => _coins;
        set => _coins = value;
    }

    public override void UseTerminal()
    {
        CharacterInRange.GetComponent<CharacterInventory>().AddCoins(Random.Range(_coins - 3, _coins + 3));
    }
}
