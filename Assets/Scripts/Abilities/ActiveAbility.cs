using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : Ability
{
    public float cooldown;
    public bool unlimitedUses;
    public int startingUses;

    public virtual void Trigger(Transform source)
    {
    }
}