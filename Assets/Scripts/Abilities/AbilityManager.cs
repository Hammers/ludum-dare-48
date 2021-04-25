using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    [SerializeField] private Ability[] abilities;
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
        if(Input.GetButtonUp("Fire1"))
            leftClickAbility?.Trigger(player);
        else if(Input.GetButtonUp("Fire2"))
            rightClickAbility?.Trigger(player);
    }

    public void RunCoroutine(IEnumerator co){
        StartCoroutine(co);
    }
}
