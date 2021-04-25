using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private ShopItemUI shopItemUiPrefab;
    [SerializeField] private Transform shopItemParent;

    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private TMP_Text itemDescLabel;
    [SerializeField] private TMP_Text costLabel;
    [SerializeField] private TMP_Text buyButtonLabel;
    [SerializeField] private TMP_Text coinsLabel;

    private List<ShopItem> availableItems;
    private List<ShopItem> ownedItems;
    private Action<ShopItem> purchaseCallback;
    private ShopItem selectedItem;
    private int coins;

    public void Init(List<ShopItem> availableItems, List<ShopItem> ownedItems, int coins, Action<ShopItem> purchaseCallback, Action closeCallback)
    {
        this.coins = coins;
        this.availableItems = availableItems;
        this.ownedItems = ownedItems;
        this.purchaseCallback = purchaseCallback;
        closeButton.onClick.AddListener(() => closeCallback());
        buyButton.onClick.AddListener(() => TryPurchaseItem());

        RefreshUI();
    }

    private void RefreshUI()
    {
        coinsLabel.text = coins.ToString();
        foreach(Transform child in shopItemParent){
            Destroy(child.gameObject);
        }

        bool selectedSomething = false;
        // Only show items that have the requisites unlocked
        foreach(ShopItem item in availableItems.Where(x => x.requirement == null || ownedItems.Contains(x.requirement))){
            if(!selectedSomething){
                SelectItem(item);
                selectedSomething = true;
            }
            Instantiate<ShopItemUI>(shopItemUiPrefab, shopItemParent).Setup(item, ()=> SelectItem(item));
        }
    }

    public void UpdateOwnedItems(List<ShopItem> ownedItems, int coins)
    {
        this.coins = coins;
        this.ownedItems = ownedItems;
        RefreshUI();
    }

    private void SelectItem(ShopItem item)
    {
        selectedItem = item;
        itemNameLabel.text = item.itemName;
        itemDescLabel.text = item.itemDescription;
        costLabel.text = "COST: "+item.cost.ToString();
        if(ownedItems.Contains(item)){
            buyButtonLabel.text = "OWNED";
            buyButton.enabled =  false;
        }
        else{
            buyButtonLabel.text = "BUY";
            buyButton.enabled =  coins >= item.cost;
        }
    }

    private void TryPurchaseItem()
    {
        purchaseCallback(selectedItem);
    }
}
