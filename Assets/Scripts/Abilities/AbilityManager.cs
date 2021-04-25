using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    private List<Ability> availableAbilities = new List<Ability>();
    // Start is called before the first frame update

    [SerializeField] private Ability leftClickAbility;
    [SerializeField] private Ability rightClickAbility;

    private Transform player;

    void Awake(){
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<Character>().transform;
    }

    // Update is called once per frame
    public void Update(){
        if(Input.GetButtonUp("Fire1") && availableAbilities.Contains(leftClickAbility))
            leftClickAbility?.Trigger(player);
        else if(Input.GetButtonUp("Fire2") && availableAbilities.Contains(rightClickAbility))
            rightClickAbility?.Trigger(player);
    }

    public void RunCoroutine(IEnumerator co){
        StartCoroutine(co);
    }

    public void AddAbility(Ability ability)
    {
        availableAbilities.Add(ability);
    }
}
