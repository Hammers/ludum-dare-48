using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopUI _uiPrefab;
    [SerializeField] private List<ShopItem> availableItems;
    [SerializeField] private List<ShopItem> ownedItems;

    private Transform uiParent;
    private ShopUI _activeUi;
    private Action closeCallback;
    void Start()
    {
        uiParent = GameObject.Find("HUD").transform;
    }

    private int GetPlayerCoins(){
        return FindObjectOfType<CharacterInventory>().GetCoins();
    }
    public void OpenShop(Action closeCallback)
    {
        if(_activeUi != null)
            return;

        this.closeCallback = closeCallback;
        _activeUi = Instantiate<ShopUI>(_uiPrefab, uiParent);
        _activeUi.Init(availableItems, ownedItems, GetPlayerCoins(), PurchaseItem, CloseShop);
    }

    public void CloseShop()
    {
        if(_activeUi == null)
            return;

        Destroy(_activeUi.gameObject);
        _activeUi = null;

        closeCallback();
    }

    private void PurchaseItem(ShopItem item)
    {
        if(GetPlayerCoins() < item.cost)
            return;
        
        if(ownedItems.Contains(item))
            return;

        PlayerBank.instance.coins -= item.cost;
        ownedItems.Add(item);
        item.Apply();
        _activeUi.UpdateOwnedItems(ownedItems, PlayerBank.instance.coins);
    }
}
