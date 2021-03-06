using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilitySlot{
    LeftClick,
    RightClick,
    Passive
}

public class AbilityManager : MonoBehaviour
{
    public struct ActiveAbilityState
    {
        public float currentCooldown;
        public int usesLeft;
    }

    public static AbilityManager instance;
    private Dictionary<AbilitySlot, Ability> activeAbilities = new Dictionary<AbilitySlot, Ability>();

    public ActiveAbilityState leftAbilityState;
    public ActiveAbilityState rightAbilityState;

    private Transform player;
    private bool isEnabled = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<Character>().transform;
    }

    // Update is called once per frame
    public void Update()
    {
        if(!isEnabled)
            return;

        leftAbilityState.currentCooldown = Mathf.Max(leftAbilityState.currentCooldown - Time.deltaTime, 0f);
        rightAbilityState.currentCooldown = Mathf.Max(rightAbilityState.currentCooldown - Time.deltaTime, 0f);
        
        if(Input.GetButtonDown("Fire1") && leftAbilityState.currentCooldown <= 0f && activeAbilities.ContainsKey(AbilitySlot.LeftClick)){
            var ability = activeAbilities[AbilitySlot.LeftClick] as ActiveAbility;
            if(ability != null)
            {
                if(ability.unlimitedUses){
                    ability.Trigger(player);
                    leftAbilityState.currentCooldown = ability.cooldown;
                }
                else if(leftAbilityState.usesLeft > 0){
                    leftAbilityState.usesLeft--;
                    ability.Trigger(player);
                    leftAbilityState.currentCooldown = ability.cooldown;
                }
            }
        }
        else if(Input.GetButtonDown("Fire2") && activeAbilities.ContainsKey(AbilitySlot.RightClick)){
            var ability = activeAbilities[AbilitySlot.RightClick] as ActiveAbility;
            if(ability != null)
            {
                if(ability.unlimitedUses){
                    ability?.Trigger(player);
                    rightAbilityState.currentCooldown = ability.cooldown;
                }
                else if(rightAbilityState.usesLeft > 0){
                    rightAbilityState.usesLeft--;
                    ability?.Trigger(player);
                    rightAbilityState.currentCooldown = ability.cooldown;
                }
            }
        }
    }

    public void RunCoroutine(IEnumerator co){
        StartCoroutine(co);
    }

    public void SetEnabled(bool isEnabled){
        this.isEnabled = isEnabled;
    }

    public void AddAbility(AbilitySlot slot, Ability ability)
    {
        if(ability == null){
            if(activeAbilities.ContainsKey(slot)){
                if(slot == AbilitySlot.Passive)
                    (activeAbilities[slot] as PassiveAbility).Remove();
                activeAbilities.Remove(slot);
            }
            return;
        }

        if(activeAbilities.ContainsKey(slot)){
            if(slot == AbilitySlot.Passive)
                (activeAbilities[slot] as PassiveAbility).Remove();
            activeAbilities[slot] = ability;
        }
        else activeAbilities.Add(slot, ability);
        switch(slot){
            case AbilitySlot.Passive:
                (activeAbilities[slot] as PassiveAbility).Add();
                break;
            case AbilitySlot.LeftClick:
                ActiveAbility activeAbility = ability as ActiveAbility;
                leftAbilityState.usesLeft = activeAbility.startingUses;
                leftAbilityState.currentCooldown = 0f;
                break;
            case AbilitySlot.RightClick:
                ActiveAbility rightActiveAbility = ability as ActiveAbility;
                rightAbilityState.usesLeft = rightActiveAbility.startingUses;
                rightAbilityState.currentCooldown = 0f;
                break;
        }
    }

    public Dictionary<AbilitySlot, Ability> GetEquippedAbilities(){
        return activeAbilities;
    }

    public Ability GetEquippedAbility(AbilitySlot slot)
    {
        return activeAbilities.TryGetValue(slot,out var ability) ? ability : null;
    }
    
    public void ResetUses(){
        if(activeAbilities.ContainsKey(AbilitySlot.LeftClick)){
            leftAbilityState.usesLeft = (activeAbilities[AbilitySlot.LeftClick] as ActiveAbility).startingUses;
            leftAbilityState.currentCooldown = 0f;
        }
        if(activeAbilities.ContainsKey(AbilitySlot.RightClick)){
            rightAbilityState.usesLeft = (activeAbilities[AbilitySlot.RightClick] as ActiveAbility).startingUses;
            rightAbilityState.currentCooldown = 0f;
        }
    }
}
