using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{

    [SerializeField] private AbilitySlot _slot;
    [SerializeField] private TextMeshProUGUI _abilityName;
    [SerializeField] private GameObject _bar;
    [SerializeField] private Image _barFill;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _cooldownColor;
    public void Update()
    {
        ActiveAbility ability = AbilityManager.instance.GetEquippedAbility(_slot) as ActiveAbility;
        if (ability == null)
        {
            _abilityName.text = "";
            _bar.gameObject.SetActive(false);
        }
        else
        {
            _abilityName.text = ability.abilityNameBase;
            _bar.gameObject.SetActive(true);
            AbilityManager.ActiveAbilityState state;
            switch (_slot)
            {
                case AbilitySlot.LeftClick:
                    state = AbilityManager.instance.leftAbilityState;
                    break;
                default:
                    state = AbilityManager.instance.rightAbilityState;
                    break;
            }

            if (state.currentCooldown > 0)
            {
                _barFill.color = _cooldownColor;
                _barFill.fillAmount = 1 - (state.currentCooldown / ability.cooldown);
            }
            else
            {
                _barFill.color = _activeColor;
                _barFill.fillAmount = ability.unlimitedUses ? 1f : (float)state.usesLeft / ability.startingUses;
            }
        }
    }
}
