using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Item/Ability")]
public class ShopItemAbility : ShopItem
{
    [SerializeField] private Ability ability;
    public override void Apply()
    {
        AbilityManager.instance.AddAbility(ability);
    }
}
