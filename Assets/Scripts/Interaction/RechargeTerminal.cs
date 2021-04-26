using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeTerminal : Terminal
{
    protected override void UseTerminal()
    {
        _used = true;
        _spriteRenderer.sprite = _inActiveSprite;
        GameManager.Instance.AddUsedTerminal(this);
        AbilityManager.instance.ResetUses();
        EndActivation();
    }
}
