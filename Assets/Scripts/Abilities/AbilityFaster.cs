using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ability/Passive/MovementSpeed")]
public class AbilityFaster : PassiveAbility
{
    public enum ModifierState{
        Addend,
        Multiplier
    }

    [SerializeField] private ModifierState modifierType;
    [SerializeField] private float amount;

    public override void Add()
    {
        switch(modifierType){
            case ModifierState.Addend:
                FindObjectOfType<CharacterMovement>().AddMovementAddend(abilityName, amount);
                break;
            case ModifierState.Multiplier:
                FindObjectOfType<CharacterMovement>().AddMovementMultiplier(abilityName, amount);
                break;
        }
    }

    public override void Remove()
    {
        switch(modifierType){
            case ModifierState.Addend:
                FindObjectOfType<CharacterMovement>().RemoveMovementAddend(abilityName);
                break;
            case ModifierState.Multiplier:
                FindObjectOfType<CharacterMovement>().RemoveMovementMultiplier(abilityName);
                break;
        }
    }
}
