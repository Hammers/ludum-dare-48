using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text itemLabel;

    private ShopItem shopItem;
    public void Setup(ShopItem shopItem, Action callback)
    {
        button.onClick.AddListener(() => callback());
        itemLabel.text = shopItem.itemName;
    }
}
