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

    private Ability ability;
    public void Setup(bool selected, Ability ability, Action callback)
    {
        if(selected)
            button.Select();

        button.onClick.AddListener(() => {
            button.Select();
            callback();
        });
        itemLabel.text = ability.abilityName;
    }
}
