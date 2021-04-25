using UnityEngine;

public class PlayerBank : MonoBehaviour
{
    public static PlayerBank instance;

    public int coins;

    public Ability leftClickAbility;
    public Ability rightClickAbility;


    // Start is called before the first frame update
    void Start()
    {
        if(PlayerBank.instance != null){
            Destroy(gameObject);
            return;
        }

        PlayerBank.instance = this;
    }
}
