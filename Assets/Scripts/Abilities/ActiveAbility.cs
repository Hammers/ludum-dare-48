using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability
{
    public float cooldown;
    public bool unlimitedUses;
    public int startingUses;
    public abstract void Trigger(Transform source);
}
