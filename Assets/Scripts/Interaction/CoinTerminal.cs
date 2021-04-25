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

    protected override void UseTerminal()
    {
        GameManager.Instance.ActivateCoinTerminal(this);
    }

    public void CancelActivation()
    {
        EndActivation();
    }
    
    public void AcceptActivation()
    {
        _used = true;
        _spriteRenderer.sprite = _inActiveSprite;
        GameManager.Instance.AddUsedTerminal(this);
        CharacterInRange.GetComponent<CharacterInventory>().AddCoins(Random.Range(_coins - 3, _coins + 3));
        EndActivation();
    }
}
