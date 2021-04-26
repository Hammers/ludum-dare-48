using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    // Slot Pane Stuff
    [Header("Slot Pane")]
    [SerializeField] private GameObject slotPane;
    [SerializeField] private Button leftSlotButton;
    [SerializeField] private Button rightSlotButton;
    [SerializeField] private Button passiveSlotButton;
    [SerializeField] private TMP_Text leftSlotLabel;
    [SerializeField] private TMP_Text rightSlotLabel;
    [SerializeField] private TMP_Text passiveSlotLabel;
    [SerializeField] private Button endGameButton;
    [SerializeField] private TextMeshProUGUI endGameButtonText;
    [SerializeField] private int speedboatAmount = 10000;

    // Slot Selection Pane Stuff
    [Header("Slot Selection Pane")]
    [SerializeField] private GameObject selectAbilityPane;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button doneButton;
    [SerializeField] private ShopItemUI shopItemUiPrefab;
    [SerializeField] private Transform shopItemParent;

    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private TMP_Text itemDescLabel;
    [SerializeField] private TMP_Text costLabel;
    [SerializeField] private TMP_Text buyButtonLabel;
    [SerializeField] private TMP_Text coinsLabel;

    Dictionary<AbilitySlot, Ability> equippedAbilities;
    private List<Ability> availableAbilities;
    private List<Ability> ownedAbilities;
    private Action<Ability> purchaseCallback;
    private Action<AbilitySlot, Ability> setCallback;
    private Ability selectedAbility;
    private int coins;
    
    bool slotSelected = false;
    AbilitySlot currentSlot;

    public void Init(Dictionary<AbilitySlot, Ability> equippedAbilities, List<Ability> availableAbilities, List<Ability> ownedAbilities, int coins, 
        Action<Ability> purchaseCallback, Action<AbilitySlot, Ability> setCallback, Action closeCallback)
    {
        this.coins = coins;
        this.equippedAbilities = equippedAbilities;
        this.availableAbilities = availableAbilities;
        this.ownedAbilities = ownedAbilities;
        this.purchaseCallback = purchaseCallback;
        this.setCallback = setCallback;
        coinsLabel.text = coins.ToString();
        closeButton.onClick.AddListener(() => closeCallback());
        buyButton.onClick.AddListener(() => TryPurchaseItem());
        doneButton.onClick.AddListener(() => TrySelectItem());
        endGameButton.gameObject.SetActive(PlayerBank.instance.passedEndGameThreshold);
        endGameButtonText.text = $"Buy a speedboat - {speedboatAmount}";
        leftSlotButton.onClick.AddListener(() => {
            slotSelected = true;
            currentSlot = AbilitySlot.LeftClick;
            RefreshUI();
        });

        rightSlotButton.onClick.AddListener(() => {
            slotSelected = true;
            currentSlot = AbilitySlot.RightClick;
            RefreshUI();
        });

        passiveSlotButton.onClick.AddListener(() => {
            slotSelected = true;
            currentSlot = AbilitySlot.Passive;
            RefreshUI();
        });

        RefreshUI();
    }

    private void RefreshSlotPane()
    {
        if(equippedAbilities.ContainsKey(AbilitySlot.LeftClick)){
            leftSlotLabel.text = equippedAbilities[AbilitySlot.LeftClick].abilityNameBase;
        }
        else{
            leftSlotLabel.text = "+";
        }

        if(equippedAbilities.ContainsKey(AbilitySlot.RightClick)){
            rightSlotLabel.text = equippedAbilities[AbilitySlot.RightClick].abilityNameBase;
        }
        else{
            rightSlotLabel.text = "+";
        }

        if(equippedAbilities.ContainsKey(AbilitySlot.Passive)){
            passiveSlotLabel.text = equippedAbilities[AbilitySlot.Passive].abilityNameBase;
        }
        else{
            passiveSlotLabel.text = "+";
        }
    }

    private void RefreshSlotSelectionPane()
    {
        coinsLabel.text = coins.ToString();
        foreach(Transform child in shopItemParent){
            Destroy(child.gameObject);
        }

        List<Ability> abilitiesToShow = new List<Ability>();

        // Only show items that have the requisites unlocked
        foreach(Ability ability in availableAbilities.Where(x => currentSlot == AbilitySlot.Passive ? x is PassiveAbility : x is ActiveAbility))
        {
            // If any upgrades already made it in, leave this out
            if(abilitiesToShow.Count(x => x.IsUpgradeOf(ability)) > 0)
                continue;

            // If there is no previous, add it in
            if(ability.previous == null){
                abilitiesToShow.Add(ability);
                continue;
            }

            // Check if the previous version is already owned
            if(!ownedAbilities.Contains(ability.previous))
                continue;

            // Finally, check if an ancestor is already in the list, and remove it if so.
            Ability previous = abilitiesToShow.FirstOrDefault(x => ability.IsUpgradeOf(x));
            if(previous != null)
                abilitiesToShow.Remove(previous);

            abilitiesToShow.Add(ability);
        }

        // If there was already a selection, try to keep that active
        if(selectedAbility != null && !abilitiesToShow.Contains(selectedAbility))
            selectedAbility = abilitiesToShow.FirstOrDefault(x => x.IsUpgradeOf(selectedAbility));

        foreach(Ability ability in abilitiesToShow)
        {
            if(selectedAbility == null)
                selectedAbility = ability;
            
            Instantiate<ShopItemUI>(shopItemUiPrefab, shopItemParent).Setup(selectedAbility == ability, ability, ()=> SelectItem(ability));
        }
        SelectItem(selectedAbility);
    }

    private void RefreshUI()
    {
        if(slotSelected)
        {
            slotPane.SetActive(false);
            selectAbilityPane.SetActive(true);
            RefreshSlotSelectionPane();
        }
        else
        {
            slotPane.SetActive(true);
            selectAbilityPane.SetActive(false);
            RefreshSlotPane();
        }
    }

    public void UpdateOwnedItems(Dictionary<AbilitySlot, Ability> activeAbilities, List<Ability> ownedAbilities, int coins)
    {
        this.coins = coins;
        this.ownedAbilities = ownedAbilities;
        this.equippedAbilities = activeAbilities;
        RefreshUI();
    }

    private void SelectItem(Ability ability)
    {
        selectedAbility = ability;
        itemNameLabel.text = ability.abilityName;
        itemDescLabel.text = ability.abilityDesc;
        costLabel.text = "COST: "+ability.cost.ToString();

        if(ownedAbilities.Contains(ability)){
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButtonLabel.text = ability.previous == null ? "BUY" : "UPGRADE";
            buyButton.enabled =  coins >= ability.cost;
        }
    }

    private void TryPurchaseItem()
    {
        if(equippedAbilities.ContainsKey(currentSlot) && equippedAbilities[currentSlot] == selectedAbility){
            return;
        }

        if(ownedAbilities.Contains(selectedAbility))
            return;
        
        purchaseCallback(selectedAbility);
    }
    private void TrySelectItem()
    {
        slotSelected = false;
        RefreshUI();
        if(!equippedAbilities.ContainsKey(currentSlot) || equippedAbilities[currentSlot] != selectedAbility){
            setCallback(currentSlot, selectedAbility);
        }
    }

    public void OnEndGamePressed()
    {
        if (PlayerBank.instance.coins > speedboatAmount)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            coinsLabel.color = Color.red;
            coinsLabel.DOColor(Color.white, 0.5f);
        }
    }
}
