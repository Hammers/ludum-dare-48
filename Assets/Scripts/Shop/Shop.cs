using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopUI _uiPrefab;
    [SerializeField] private List<Ability> availableAbilities;
    [SerializeField] private List<Ability> ownedAbilities;

    private Transform uiParent;
    private ShopUI _activeUi;
    private Action closeCallback;
    void Start()
    {
        uiParent = GameObject.Find("HUD").transform;
    }

    private int GetPlayerCoins(){
        return PlayerBank.instance.coins;
    }
    public void OpenShop(Action closeCallback)
    {
        if(_activeUi != null)
            return;

        this.closeCallback = closeCallback;
        _activeUi = Instantiate<ShopUI>(_uiPrefab, uiParent);
        _activeUi.Init(AbilityManager.instance.GetEquippedAbilities(), availableAbilities, ownedAbilities, GetPlayerCoins(), PurchaseItem, SetAbility, CloseShop);
    }

    public void CloseShop()
    {
        if(_activeUi == null)
            return;

        Destroy(_activeUi.gameObject);
        _activeUi = null;

        closeCallback();
    }

    private void PurchaseItem(Ability ability)
    {
        if(GetPlayerCoins() < ability.cost)
            return;
        
        if(ownedAbilities.Contains(ability))
            return;

        PlayerBank.instance.coins -= ability.cost;
        ownedAbilities.Add(ability);
        _activeUi.UpdateOwnedItems(AbilityManager.instance.GetEquippedAbilities(), ownedAbilities, PlayerBank.instance.coins);
    }

    private void SetAbility(AbilitySlot slot, Ability ability)
    {
        // Get the current slots and see if this ability or a previous version of it are already equipped
        var equippedAbilities = AbilityManager.instance.GetEquippedAbilities();
        AbilitySlot slotToSwap = slot;
        foreach(var pair in equippedAbilities){
            if(ability == pair.Value || ability.IsUpgradeOf(pair.Value))
            {
                slotToSwap = pair.Key;
                break;
            }
        }
        // Move the contents of this slot to the slot where this ability already is.
        if(slotToSwap != slot){
            if(equippedAbilities.ContainsKey(slot))
                AbilityManager.instance.AddAbility(slotToSwap, equippedAbilities[slot]);
            else AbilityManager.instance.AddAbility(slotToSwap, null);
        }

        Ability abilityToAdd = ownedAbilities.FirstOrDefault(x => x.IsUpgradeOf(ability));
        if(abilityToAdd == null){
            if(ownedAbilities.Contains(ability))
                abilityToAdd = ability;
            else
                abilityToAdd = ownedAbilities.FirstOrDefault(x => ability.IsUpgradeOf(x));
        }

        if(abilityToAdd == null)
            return;

        Ability higherUpgrade = abilityToAdd;
        while(higherUpgrade != null){
            higherUpgrade = ownedAbilities.FirstOrDefault(x => x.IsUpgradeOf(abilityToAdd));
            if(higherUpgrade != null)
                abilityToAdd = higherUpgrade;
        }

        AbilityManager.instance.AddAbility(slot, abilityToAdd);
        _activeUi.UpdateOwnedItems(AbilityManager.instance.GetEquippedAbilities(), ownedAbilities, PlayerBank.instance.coins);
    }
}
